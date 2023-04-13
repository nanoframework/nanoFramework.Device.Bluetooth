//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// The BluetoothLEAdvertisementPublisher class allows the configuration and 
    /// advertising of a Bluetooth LE advertisement packet.
    /// </summary>
    public class BluetoothLEAdvertisementPublisher
    {
        private readonly BluetoothLEAdvertisement _advertisement;
        private BluetoothLEAdvertisementPublisherStatus _status;
        private bool _useExtendedAdvertisement;
        private bool _includeTransmitPowerLevel;
        private bool _isAnonymous;
        private short _preferredTransmitPowerLevelInDBm;
        private bool _dataNotFitInAdvertisement = false;

        private static short _advertisementInstance = 0;  // Extended only

        /// <summary>
        /// Delegate for notification that the status of the BluetoothLEAdvertisementPublisher has changed.
        /// </summary>
        /// <param name="sender">BluetoothLEAdvertisementPublisher class sending event.</param>
        /// <param name="args">Event arguments.</param>
        public delegate void BluetoothLEAdvertisementPublisherHandler(Object sender, BluetoothLEAdvertisementPublisherStatusChangedEventArgs args);

        /// <summary>
        /// Notification that the status of the BluetoothLEAdvertisementPublisher has changed.
        /// </summary>
        public event BluetoothLEAdvertisementPublisherHandler StatusChanged;

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementPublisher object.
        /// </summary>
        public BluetoothLEAdvertisementPublisher(): this(new BluetoothLEAdvertisement())
        {
        }

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementPublisher object with the Bluetooth LE advertisement to publish.
        /// </summary>
        /// <param name="advertisement">The Bluetooth LE advertisement to publish.</param>
        public BluetoothLEAdvertisementPublisher(BluetoothLEAdvertisement advertisement)
        {
            _advertisement = advertisement;
            _status = BluetoothLEAdvertisementPublisherStatus.Created;
            _useExtendedAdvertisement = false;
            _includeTransmitPowerLevel = false;
            _isAnonymous = false;
        }

        /// <summary>
        /// Gets a copy of the Bluetooth LE advertisement to publish.
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get => _advertisement; }

        /// <summary>
        /// Specifies whether the transmit power level is included in the advertisement header. 
        /// Defaults to False.
        /// </summary>
        public bool IncludeTransmitPowerLevel { get => _includeTransmitPowerLevel; set => _includeTransmitPowerLevel = value; }

        /// <summary>
        /// Specifies whether or not the device address is included in the advertisement header. 
        /// By default, the address is included.
        /// </summary>
        public bool IsAnonymous { get => _isAnonymous; set => _isAnonymous = value; }

        /// <summary>
        /// If specified, requests that the radio use the indicated transmit power level for the advertisement. 
        /// Defaults to 0.        
        /// </summary>
        public short PreferredTransmitPowerLevelInDBm { get => _preferredTransmitPowerLevelInDBm; set => _preferredTransmitPowerLevelInDBm = value; }

        /// <summary>
        /// Gets the current status of the BluetoothLEAdvertisementPublisher.
        /// </summary>
        public BluetoothLEAdvertisementPublisherStatus Status 
        { 
            get => _status; 
            internal set 
            {
                if (_status == value)
                    return;

                _status = value;

                StatusChanged?.Invoke(this, 
                    new BluetoothLEAdvertisementPublisherStatusChangedEventArgs(_status, 0, _preferredTransmitPowerLevelInDBm));
            }
        }

        /// <summary>
        /// Specifies that the advertisement publisher should use the Extended Advertising format.
        /// Defaults to False, use legacy advertisements. If Bluetooth 5.0 not available on target platform
        /// then a PlatformNotSupportedException will be thrown.
        /// </summary>
        public bool UseExtendedAdvertisement
        {
            get => _useExtendedAdvertisement;
            set
            {
                if (value && !NativeIsExtendedAdvertisingAvailable())
                {
                    throw new PlatformNotSupportedException();
                }
                _useExtendedAdvertisement = value;
            }
        }

        /// <summary>
        /// Flag for internal use by Gatt service provider.
        /// Indicates that all data didn't fix in 31 byte advertisement or scan response.
        /// </summary>
        internal bool DataNotFitInAdvertisement { get => _dataNotFitInAdvertisement; }

        /// <summary>
        /// Start advertising a Bluetooth LE advertisement payload.
        /// </summary>
        public void Start()
        {
            byte[] advertData;
            byte[] scanResponse;

            // Check and switch to server mode
            if (BluetoothNanoDevice.RunMode != BluetoothNanoDevice.Mode.Server)
            {
                BluetoothLEServer.Instance.Start();
            }

            // Make sure pairing attributes set
            BluetoothLEServer.Instance.Pairing.SetPairAttributes();

            Status = BluetoothLEAdvertisementPublisherStatus.Waiting;

            if (_useExtendedAdvertisement)
            {
                //TODO extended advertisement currently not supported

                _advertisementInstance++; // Increment instance value

                //_advertisement.CreateExtendedAdvertisement(out advertData);
                throw new NotSupportedException();

                //if (NativeStartExtendedAdvertising(_advertisementInstance, advertData))
                //{
                //    Status = BluetoothLEAdvertisementPublisherStatus.Started;
                //}
            }
            else
            {
                _dataNotFitInAdvertisement = _advertisement.CreateLegacyAdvertisements(out advertData, out scanResponse);
                if (NativeStartLegacyAdvertising(advertData, scanResponse))
                {
                    Status = BluetoothLEAdvertisementPublisherStatus.Started;
                }
            }
        }

        /// <summary>
        /// Stop the publisher and stop advertising a Bluetooth LE advertisement payload.
        /// </summary>
        public void Stop()
        {
            NativeStopAdvertising();
            Status = BluetoothLEAdvertisementPublisherStatus.Stopped;
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartLegacyAdvertising(byte[] advertData, byte[] scanResponse);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartExtendedAdvertising(short instance, byte[] advertData);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertising();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeIsExtendedAdvertisingAvailable();

        #endregion
    }
}