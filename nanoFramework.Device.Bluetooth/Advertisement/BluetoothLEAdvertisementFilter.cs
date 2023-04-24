//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Groups parameters used to configure payload-based filtering of received Bluetooth
    /// LE advertisements.
    /// </summary>
    public class BluetoothLEAdvertisementFilter 
    {
        /// <summary>
        /// Creates a new BluetoothLEAdvertisementFilter object.
        /// </summary>
        public BluetoothLEAdvertisementFilter()
        {
        }

        // TODO
        ///// <summary>
        ///// A BluetoothLEAdvertisement object that can be applied as filters to received
        ///// Bluetooth LE advertisements.
        ///// </summary>
        //public BluetoothLEAdvertisement Advertisement { get; set; }

        // TODO
        ///// <summary>
        ///// Gets a vector of byte patterns with offsets to match advertisement sections in
        ///// a received Bluetooth LE advertisement.
        ///// </summary>
        //public IList<BluetoothLEAdvertisementBytePattern> BytePatterns { get; }


        internal bool Filter(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            return true;
        }

    }
}