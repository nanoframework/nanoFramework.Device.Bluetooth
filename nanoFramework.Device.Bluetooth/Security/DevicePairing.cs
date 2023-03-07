//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Device.Bluetooth.Security;
using System;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Contains information and enables pairing for a device.
    /// </summary>
    public sealed class DevicePairing
    {
        BluetoothLEDevice _device = null;
        BluetoothLEServer _server = null;
        private readonly AutoResetEvent _completedEvent = new(false);

        bool _canPair = true;
        bool _isPaired;
        bool _isAuthenticated;

        // IO Capibilities used for pairing
        // Defualt to NoInputNoOutput
        DevicePairingIOCapabilities _ioCapabilities = DevicePairingIOCapabilities.NoInputNoOutput;

        // Protection level to use when pairing
        DevicePairingProtectionLevel _protectionLevel = DevicePairingProtectionLevel.None;

        // Allow device to be bonded
        private bool _bondingAllowed;

        private DevicePairingResultStatus _pairingStatus;

        // Use external method for information for pairing process. 
        private bool _outOfBand = false;

        // Time for Pairing to complete before reporting timeout
        private const int operationPairTimeout = 10000;

        /// <summary>
        /// Delegate for Pairing requested events.
        /// </summary>
        /// <param name="sender">Pairing class sending event.</param>
        /// <param name="args">Event arguments.</param>
        public delegate void DevicePairingRequestedHandler(Object sender, DevicePairingRequestedEventArgs args);

        /// <summary>
        /// Delegate for Pairing Complete events.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="args">Device event arguments.</param>
        public delegate void PairingCompleteHandler(Object sender, DevicePairingEventArgs args);

        /// <summary>
        /// Event raised when a pairing action is requested.
        /// </summary>
        public event DevicePairingRequestedHandler PairingRequested;

        /// <summary>
        /// Event fired when a Pairing has completed. Check status for succesful completion
        /// and the IsPaired, IsAuthenticated.
        /// </summary>
        public event PairingCompleteHandler PairingComplete;

        internal DevicePairing(BluetoothLEDevice device)
        {
            _device = device;
            Reset();
        }

        internal DevicePairing(BluetoothLEServer server)
        {
            _server = server;

            Reset();

            // Set default attributes in Native
            SetPairAttributes();
        }

        internal void Reset()
        {
            _ioCapabilities = DevicePairingIOCapabilities.NoInputNoOutput;
            _protectionLevel = DevicePairingProtectionLevel.None;
            _isPaired = false;
            _isAuthenticated = false;
            _outOfBand = false;
            _bondingAllowed = true;
        }

        /// <summary>
        /// When true will allow device to be bonded.
        /// See Bonding methods. 
        /// Defaults to true. 
        /// </summary>
        public bool AllowBonding 
        { 
            get => _bondingAllowed; 
            set
            {
                _bondingAllowed = value;
                SetPairAttributes();
            }
        }

        /// <summary>
        /// True if uses external method for exchange of information for pairing process.
        /// Could be predefined info in device.
        /// Defaults to false.
        /// </summary>
        public bool OutOfBand { get => _outOfBand; set => _outOfBand = value; }

        /// <summary>
        /// Gets or sets protection level to be used for pairing.
        /// The default value is set based on the protection level requirements in added characteristics and descriptors.
        /// </summary>
        public DevicePairingProtectionLevel ProtectionLevel 
        { 
            get => _protectionLevel;
            set
            { 
                _protectionLevel = value;
                SetPairAttributes();
            }
        }

        /// <summary>
        /// Gets or sets the IO capabilities of the device.
        /// By default the IO capabilties are set to NoInputNoOutput which will cause Unauthenicated Just Wroks pairing.
        /// </summary>
        public DevicePairingIOCapabilities IOCapabilities 
        { 
            get => _ioCapabilities;
            set
            { 
                _ioCapabilities = value;
                SetPairAttributes();
            }
        }

        /// <summary>
        /// Attempts to initiate a pairing of device using default protecion level.
        /// </summary>
        /// <returns>The result of the device pairing action.</returns>
        public DevicePairingResult Pair()
        {
            return Pair(_protectionLevel);
        }

        /// <summary>
        /// Attempts to pair the device using a provided level of protection.
        /// </summary>
        /// <param name="minProtectionLevel">The required level of protection to use for the pairing action.</param>
        /// <returns>The result of the pairing action.</returns>
        public DevicePairingResult Pair(DevicePairingProtectionLevel minProtectionLevel)
        {
            _isPaired = false;

            if (_server != null)
            {
                _server.Start();
            }
            else
            {
                _device.ConnectDeviceIfNotConnected();
            }

            SetPairAttributes();
            
            if (NativeStartPair(_device.ConnectionHandle) == 0)
            { 
                if (!_completedEvent.WaitOne(operationPairTimeout, false))
                {
                    _pairingStatus = DevicePairingResultStatus.AuthenticationTimeout;
                }
            }

            return new DevicePairingResult(minProtectionLevel, _pairingStatus);
        }

        /// <summary>
        /// Unpairs the device.
        /// </summary>
        /// <returns>The result of the unpairing action.</returns>
        public DeviceUnpairingResult Unpair()
        {
            return new DeviceUnpairingResult(0);
        }

        /// <summary>
        ///  Gets a value that indicates whether the device can be paired.
        ///  **True** if the device can be paired, otherwise **false**.
        /// </summary>
        public bool CanPair { get => _canPair; }

        /// <summary>
        /// Gets a value that indicates whether the device is currently paired.
        /// **True** if the device is currently paired, otherwise **false**.
        /// </summary>
        public bool IsPaired { get => _isPaired; }

        /// <summary>
        /// Gets a value that indicates if latest pairing was Authenicated.
        /// </summary>
        public bool IsAuthenticated { get => _isAuthenticated; }

        /// <summary>
        /// Sets Pairing Attibutes in Native level.
        /// </summary>
        internal void SetPairAttributes()
        {
            NativeSetPairAttributes();
        }

        /// <summary>
        /// Internal event handler.
        /// </summary>
        /// <param name="btEvent">The Bluetooth Event.</param>
        internal void OnEvent(BluetoothEventSesssion btEvent)
        {
            switch (btEvent.type)
            {
                case BluetoothEventType.PassKeyActions:
                    DevicePairingKinds kind = (DevicePairingKinds)btEvent.data;

                    // Debug.WriteLine($"# Pairing DevicePairingKinds:{kind} status:{btEvent.status}");

                    OnEvent(btEvent, kind, 0);

                    // == Input passkey ( paaskey displayed on other devicde ) ==
                    // Enter Passkey by calling Accept with pin
                    // Event must call -> Accept with passkey

                    // Display action, Display passkey on current device so passkey can be entered on peer device. 
                    // i.e. Set the Passkey to be entered on peer.
                    // Event must call -> Accept with passkey

                    // Numbers displayed on both devices 
                    // Numeric compare
                    // Accept passkey is same as whats displayed on peer
                    // Call Accept or ignore

                    // Out of band (Not implemented)
                    // Accept by calling Accept with Password Credentials
                    break;

                case BluetoothEventType.PassKeyActionsNumericComparison:
                    OnEvent(btEvent, DevicePairingKinds.ConfirmPinMatch, btEvent.data32);
                    break;

                case BluetoothEventType.AuthenticationComplete:
                    _pairingStatus = (DevicePairingResultStatus)btEvent.status;

                    _isPaired = (_pairingStatus == DevicePairingResultStatus.Paired);

                    bool isEncrypted = (btEvent.data & 1) != 0;
                    _isAuthenticated = (btEvent.data & 2) != 0;
                    bool isBonded = (btEvent.data & 4) != 0;

                    //Debug.WriteLine($"# Pairing AuthenticationComplete isPaired:{_isPaired} status:{btEvent.status} data:{btEvent.data} isEncrypted:{isEncrypted} isAuthenticated:{isAuthenticated} isBonded:{isBonded} ");

                    // Inform Pair operation Auth complete
                    _completedEvent.Set();

                    PairingComplete?.Invoke(this, new DevicePairingEventArgs(btEvent.connectionHandle, _pairingStatus));
                    break;

                default:
                    break;
            }
        }

        internal void OnEvent(BluetoothEventSesssion btEvent, DevicePairingKinds kind, uint pin)
        {
            PairingRequested?.Invoke(_device == null ? _server : _device, new DevicePairingRequestedEventArgs(this, btEvent.connectionHandle, kind, pin));
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeStartPair(ushort connection);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeSetPairAttributes();

        #endregion
    }
}