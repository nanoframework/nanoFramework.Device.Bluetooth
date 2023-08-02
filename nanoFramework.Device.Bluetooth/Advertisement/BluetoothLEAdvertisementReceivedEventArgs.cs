//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Provides data for a Received event on a <see cref="BluetoothLEAdvertisementWatcher"/>. A <see cref="BluetoothLEAdvertisementReceivedEventArgs"/>
    /// instance is created when the Received event occurs on a <see cref="BluetoothLEAdvertisementWatcher"/> object.
    /// </summary>
    public class BluetoothLEAdvertisementReceivedEventArgs
    {
        private readonly ulong _bluetoothAddress;
        private readonly BluetoothAddressType _bluetoothAddressType;
        private BluetoothLEAdvertisement _advertisement;
        private readonly BluetoothLEAdvertisementType _advertisementType;
        private readonly short _rawSignalStrengthInDBm;
        private DateTime _timestamp;
        private byte[] _rawAdvertData = null;

        internal BluetoothLEAdvertisementReceivedEventArgs()
        {
            _advertisement = new BluetoothLEAdvertisement();
            _advertisementType = BluetoothLEAdvertisementType.ConnectableDirected;
            _rawSignalStrengthInDBm = -99;
            _timestamp = DateTime.UtcNow;
        }

        internal BluetoothLEAdvertisementReceivedEventArgs(
                ulong BluetoothAddress,
                BluetoothAddressType BluetoothAddressType,
                BluetoothLEAdvertisementType AdvertisementType,
                BluetoothLEAdvertisement Advertisement,
                short RawSignalStrengthInDBm,
                DateTime Timestamp
            )
        {
            _bluetoothAddress = BluetoothAddress;
            _bluetoothAddressType = BluetoothAddressType;
            _advertisement = Advertisement;
            _advertisementType = AdvertisementType;
            _rawSignalStrengthInDBm = RawSignalStrengthInDBm;
            _timestamp = Timestamp;
        }

        /// <summary>
        /// Create <see cref="BluetoothLEAdvertisementReceivedEventArgs"/> by calling native to fill in fields.
        /// </summary>
        /// <returns><see cref="BluetoothLEAdvertisementReceivedEventArgs"/> object.</returns>
        internal static BluetoothLEAdvertisementReceivedEventArgs CreateFromEvent(BluetoothLEAdvertisementWatcher watcher, int eventID)
        {
            BluetoothLEAdvertisementReceivedEventArgs ad = new();

            // Call native code to fill in BluetoothLEAdvertisementReceivedEventArgs and BluetoothLEAdvertisement.
            ad.NativeCreateFromEvent(eventID);

            // Parse received data into data sections and properties if available.
            if (ad._rawAdvertData != null)
            {
                ad.Advertisement.ParseBytesToSectionData(ad._rawAdvertData);
                ad._rawAdvertData = null;

                // TODO can we do this ? merge. extra memory needed, maybe just save last advert
                //// If a scan response then try to merge advertisement data with original advertisement data.
                //if (ad.AdvertisementType == BluetoothLEAdvertisementType.ScanResponse)
                //{
                //    BluetoothLEAdvertisementWatcher.DeviceItem scanitem = watcher.FindScanEntry(ad.BluetoothAddress);
                //    if (scanitem != null) 
                //    {
                //        // Original item found then update and return that instead
                //        scanitem.advert.MergeAdvertisement(ad.Advertisement);
                //        ad.Advertisement = scanitem.advert;
                //    }
                //}
            }

            return ad;
        }

        /// <summary>
        /// Gets the Bluetooth LE advertisement payload data received.
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get => _advertisement; internal set => _advertisement = value; }

        /// <summary>
        /// Gets the type of the received Bluetooth LE advertisement packet.
        /// </summary>
        public BluetoothLEAdvertisementType AdvertisementType { get => _advertisementType; }

        /// <summary>
        /// Gets the Bluetooth address of the device sending the Bluetooth LE advertisement.
        /// </summary>
        public ulong BluetoothAddress { get => _bluetoothAddress; }

        /// <summary>
        /// Get the Bluetooth address type of <see cref="BluetoothAddress"/>.
        /// </summary>
        public BluetoothAddressType BluetoothAddressType { get => _bluetoothAddressType; }

        /// <summary>
        /// Gets the received signal strength indicator (RSSI) value, in dBm, for this received
        /// Bluetooth LE advertisement event. This value could be the raw RSSI or a filtered
        /// RSSI depending on filtering settings configured through <see cref="BluetoothSignalStrengthFilter"/>.
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
