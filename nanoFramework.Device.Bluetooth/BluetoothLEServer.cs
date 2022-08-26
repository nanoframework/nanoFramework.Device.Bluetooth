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
        private bool disposedValue;
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
            this.disposedValue = false;

            Init();
        }

        private void Init()
        {
            _session = new GattSession(new BluetoothDeviceId(0));
            _pairing = new DevicePairing(this);
            _services = new();

            _bluetoothEventManager.BluetoothServer = this;
            _bluetoothEventManager.Reset();
        }

        /// <summary>
        /// Server device name, defaults to 'nanoFramework'
        /// </summary>
        public String DeviceName { get => BluetoothNanoDevice.DeviceName; set => BluetoothNanoDevice.DeviceName = value; }

        /// <summary>
        /// Get GattSession aasoicated with this server.
        /// </summary>
        public GattSession Session { get => _session; }

        #region Services

        /// <summary>
        /// Returns service provider with specifued UUID
        /// </summary>
        /// <param name="serviceUuid"></param>
        /// <returns>return GattServiceProvider object or null if not found.</returns>
        public GattServiceProvider GetServiceByUUID(Guid serviceUuid)
        {
            foreach (GattServiceProvider srvprov in _services)
            {
                if (srvprov.Service.Uuid.Equals(serviceUuid))
                {
                    return srvprov;
                }
            };

            return null;
        }

        /// <summary>
        /// Get an array of all associated services providers for this Bluetooth LE Server.
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
        /// Evaulate all services and work out security requirements
        /// </summary>
        /// <returns>Minimum DevicePairingProtectionLevel for servcies/characteristics</returns>
        private DevicePairingProtectionLevel EvaluateSecurityPairingRequirements()
        {
            DevicePairingProtectionLevel _minProtectionLevel = DevicePairingProtectionLevel.Default;

            bool secureConnection = false;
            bool authentication = false;

            foreach (GattServiceProvider srvp in _services)
            {
                foreach (GattLocalCharacteristic chr in srvp.Service.Characteristics)
                {
                    if (chr.ReadProtectionLevel == GattProtectionLevel.EncryptionRequired ||
                        chr.ReadProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired)
                    {
                        secureConnection = true;
                    }

                    if (chr.ReadProtectionLevel == GattProtectionLevel.AuthenticationRequired ||
                        chr.ReadProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.AuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired)
                    {
                        authentication = true;
                    }
                }
            }

            if (secureConnection)
            {
                _minProtectionLevel = DevicePairingProtectionLevel.Encryption;
            }

            if (authentication)
            {
                _minProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;
            }

            return _minProtectionLevel;
        }

        #endregion Security

        /// <summary>
        /// Start Bluetoth stack for server mode
        /// If already running or in Client mode will give an InvalidOperation Exception.
        /// </summary>
        public void Start()
        {
            // Check and switch to server mode ( Stack Bluetooth stack )
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.Server);
        }

        /// <summary>
        /// Stop Bluetooth server mode.
        /// This stops the bluetooth stack but doesn't remove the current Services managed objects.
        /// To remove these dispose the BluetoothLEServer instance.
        /// </summary>
        public void Stop()
        {
            // Check in Server mode and change if not
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.NotRunning);
        }

        /// <summary>
        /// Route Bluetooth events
        /// </summary>
        /// <param name="btEvent"></param>
        internal void OnEvent(BluetoothEventSesssion btEvent)
        {
            //Debug.WriteLine($"# BluetoothLEServer OnEvent, type:{btEvent.type} status:{btEvent.status}");

            switch (btEvent.type)
            {
                case BluetoothEventType.ClientConnected:
                case BluetoothEventType.ClientDisconnected:
                    _session.OnEvent(btEvent);
                    break;

                case BluetoothEventType.ClientSessionChanged:
                case BluetoothEventType.PassKeyActions:
                case BluetoothEventType.PassKeyActions_numcmp:
                case BluetoothEventType.AuthenticationComplete:
                    Pairing.OnEvent(btEvent);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Stop, ignore any exceptions
                try
                {
                    Stop();
                }
                catch {}; 

                if (disposing)
                {

                    _bluetoothEventManager.Reset();
                    _bluetoothEventManager.BluetoothServer = null;

                    _services = null;
                    _instance = null;
                }

                disposedValue = true;
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