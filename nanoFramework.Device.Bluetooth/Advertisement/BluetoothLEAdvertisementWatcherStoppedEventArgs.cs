//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Provides data for a Stopped event on a BluetoothLEAdvertisementWatcher. A BluetoothLEAdvertisementWatcherStoppedEventArgs
    /// instance is created when the Stopped event occurs on a BluetoothLEAdvertisementWatcher
    /// object.
    /// </summary>
    public class BluetoothLEAdvertisementWatcherStoppedEventArgs 
    {
        private readonly BluetoothError  _error;

        internal BluetoothLEAdvertisementWatcherStoppedEventArgs(BluetoothError error)
        {
            _error = error;
        }

        /// <summary>
        /// Gets the error status for Stopped event.
        /// </summary>
        public BluetoothError Error { get => _error; }
    }
}