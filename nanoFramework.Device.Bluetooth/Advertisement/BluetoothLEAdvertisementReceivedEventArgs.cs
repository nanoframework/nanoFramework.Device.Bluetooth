//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Provides data for a Received event on a BluetoothLEAdvertisementWatcher. A BluetoothLEAdvertisementReceivedEventArgs
    /// instance is created when the Received event occurs on a BluetoothLEAdvertisementWatcher
    /// object.
    /// </summary>
    public class BluetoothLEAdvertisementReceivedEventArgs
    {
        private readonly ulong _bluetoothAddress;
        private readonly BluetoothLEAdvertisement _advertisement;
        private readonly BluetoothLEAdvertisementType _advertisementType;
        private readonly short _rawSignalStrengthInDBm;
        private DateTime _timestamp;

        internal BluetoothLEAdvertisementReceivedEventArgs()
        {
            _advertisement = new BluetoothLEAdvertisement();
            _advertisementType = BluetoothLEAdvertisementType.ConnectableDirected;
            _rawSignalStrengthInDBm = -99;
            _timestamp = DateTime.UtcNow;
        }

        internal BluetoothLEAdvertisementReceivedEventArgs(
                ulong BluetoothAddress,
                BluetoothLEAdvertisementType AdvertisementType,
                BluetoothLEAdvertisement Advertisement,
                short RawSignalStrengthInDBm,
                DateTime Timestamp
            )
        {
            _bluetoothAddress = BluetoothAddress;
            _advertisement = Advertisement;
            _advertisementType = AdvertisementType;
            _rawSignalStrengthInDBm = RawSignalStrengthInDBm;
            _timestamp = Timestamp;
        }

        /// <summary>
        /// Create BluetoothLEAdvertisementReceivedEventArgs by calling native to fill in fields.
        /// </summary>
        /// <returns>BluetoothLEAdvertisementReceivedEventArgs object</returns>
        internal static BluetoothLEAdvertisementReceivedEventArgs CreateFromEvent(int eventID)
        {
            BluetoothLEAdvertisementReceivedEventArgs ad = new();
            ad.NativeCreateFromEvent(eventID);
            return ad;
        }


        /// <summary>
        /// Gets the Bluetooth LE advertisement payload data received.
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get => _advertisement; }

        /// <summary>
        /// Gets the type of the received Bluetooth LE advertisement packet.
        /// </summary>
        public BluetoothLEAdvertisementType AdvertisementType { get => _advertisementType; }

        /// <summary>
        /// Gets the Bluetooth address of the device sending the Bluetooth LE advertisement.
        /// </summary>
        public ulong BluetoothAddress { get => _bluetoothAddress; }

        /// <summary>
        /// Gets the received signal strength indicator (RSSI) value, in dBm, for this received
        /// Bluetooth LE advertisement event. This value could be the raw RSSI or a filtered
        /// RSSI depending on filtering settings configured through BluetoothSignalStrengthFilter.
        /// </summary>
        public short RawSignalStrengthInDBm { get => _rawSignalStrengthInDBm; }

        /// <summary>
        /// Gets the time stamp when the Received event occurred.
        /// </summary>
        public DateTime Timestamp { get => _timestamp; internal set => _timestamp = value; }

        #region Native
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeCreateFromEvent(int eventID);
        #endregion
    }
}