//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using System.Diagnostics;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE Server.
    /// </summary>
    public class BluetoothLEServer : IDisposable
    {
        private bool _disposedValue;
        private DevicePairing _pairing;
        private GattSession _session;

        private static BluetoothLEServer _instance;
        private static readonly object _lock = new object();

        internal static readonly BluetoothEventListener _bluetoothEventManager = new();
        internal static ArrayList _services = new();

        static BluetoothLEServer()
        {
        }

        /// <summary>
        /// Returns BluetoothLEServer singleton instance.
        /// </summary>
        public static BluetoothLEServer Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BluetoothLEServer();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Construct new Bluetooth Server using primary service UUID
        /// </summary>
        private BluetoothLEServer()
        {
            this._disposedValue = false;

            Init();
        }

        private void Init()
        {
            _session = new GattSession(new BluetoothDeviceId(0));
            _pairing = new DevicePairing(this);
            
            _bluetoothEventManager.BluetoothServer = this;
            _bluetoothEventManager.Reset();
        }

        /// <summary>
        /// Server device name, defaults to 'nanoFramework'.
        /// Set in the Generic Access service.
        /// </summary>
        public String DeviceName { get => BluetoothNanoDevice.DeviceName; set => BluetoothNanoDevice.DeviceName = value; }

        /// <summary>
        /// Appearance value of the device set in the Generic Access service and also the advertisment if enabled.
        /// The appearance is a 16 bit value comprising of bits 6 to 15 the device category and bits 0 to 5 the sub-categoty.
        /// See Bluetooth assigned numbers document section "Appearance Sub­category values" for values. 
        /// For example a "IOT Gateway" has a value of 0x008D.
        /// The Appearance value defaults to 0, a Generic Unknown device.
        /// </summary>
        public ushort Appearance { get => BluetoothNanoDevice.Appearance; set => BluetoothNanoDevice.Appearance = value; }

        /// <summary>
        /// Get GattSession associated with this server.
        /// </summary>
        public GattSession Session { get => _session; }

        #region Services

        /// <summary>
        /// Returns service provider with specified UUID.
        /// </summary>
        /// <param name="serviceUuid">UUID of the service to get.</param>
        /// <returns>The service with the UUID.</returns>
        public GattServiceProvider GetServiceByUUID(Guid serviceUuid)
        {
            foreach (GattServiceProvider srvprov in _services)
            {
                if (srvprov.Service.Uuid.Equals(serviceUuid))
                {
                    return srvprov;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets an array of all associated service providers for this Bluetooth LE Server.
        /// </summary>
        /// <remarks>
        /// The primary service will be index 0 followed by the Device Information at index 1.
        /// Any other Services added to server will follow these in the order they were created.
        /// </remarks>
        public GattServiceProvider[] Services { get { return (GattServiceProvider[])_services.ToArray(typeof(GattServiceProvider)); } }

        #endregion

        #region Security/Pairing

        /// <summary>
        /// Return Pairing object for handling Pairing.
        /// </summary>
        public DevicePairing Pairing { get => _pairing; }

        /// <summary>
        /// Update the pairing.ProtectionLevel if using a greater security requirement.
        /// </summary>
        /// <param name="protectionLevel"></param>
        internal void UpdateSecurityPairingRequirements(GattProtectionLevel protectionLevel)
        {
            DevicePairingProtectionLevel newProtectionLevel = DevicePairingProtectionLevel.Default;

            bool secureConnection = false;
            bool authentication = false;

            if (protectionLevel == GattProtectionLevel.EncryptionRequired ||
                protectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired )
            {
                secureConnection = true;
            }

            if (protectionLevel == GattProtectionLevel.AuthenticationRequired ||
                protectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired)
            {
                authentication = true;
            }

            if (secureConnection)
            {
                newProtectionLevel = DevicePairingProtectionLevel.Encryption;
            }

            if (authentication)
            {
                newProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;
            }

            if (newProtectionLevel > Pairing.ProtectionLevel)
            {
                Pairing.ProtectionLevel = newProtectionLevel;
            }
        }
        #endregion Security

        /// <summary>
        /// Starts Bluetooth stack for server mode.
        /// </summary>
        /// <exception cref="InvalidOperationException">If already running or in Client mode.</exception>
        public void Start()
        {
            // Check and switch to server mode ( Stack Bluetooth stack )
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.Server);
        }

        /// <summary>
        /// Stops Bluetooth server mode.
        /// This stops the bluetooth stack but doesn't remove the current Services managed objects.
        /// To remove these dispose the BluetoothLEServer instance.
        /// </summary>
        public void Stop()
        {
            // Check in Server mode and change if not
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.NotRunning);
        }

        /// <summary>
        /// Routes Bluetooth events.
        /// </summary>
        /// <param name="btEvent">The Bluetooth event.</param>
        internal void OnEvent(BluetoothEventSesssion btEvent)
        {
            //Debug.WriteLine($"# BluetoothLEServer OnEvent, type:{btEvent.type} status:{btEvent.status}");

            switch (btEvent.type)
            {
                case BluetoothEventType.ClientConnected:
                case BluetoothEventType.ClientDisconnected:
                case BluetoothEventType.ClientSessionChanged:
                    _session.OnEvent(btEvent);
                    break;

                case BluetoothEventType.PassKeyActions:
                case BluetoothEventType.PassKeyActionsNumericComparison:
                case BluetoothEventType.AuthenticationComplete:
                    Pairing.OnEvent(btEvent);
                    break;

                default:
                    break;
            }
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                // Stop, ignore any exceptions
                try
                {
                    Stop();
                }
                catch 
                { 
                    // ignore any errors
                }

                if (disposing)
                {
                    _bluetoothEventManager.Reset();
                    _bluetoothEventManager.BluetoothServer = null;

                    _services = new();
                    _instance = null;
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~BluetoothLEServer()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Dispose BluetoothLESever.  
        /// Remove all managed objects used by BluetoothLESever.
        /// </summary>
        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(disposing: true);
            // Suppress finalization.
            System.GC.SuppressFinalize(this);
        }
    }
}
