//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Represents the possible states of the BluetoothLEAdvertisementWatcher.
    /// </summary>
    public enum BluetoothLEAdvertisementWatcherStatus
    {
        /// <summary>
        /// The initial status of the watcher.
        /// </summary>
        Created = 0,

        /// <summary>
        /// The watcher is started.
        /// </summary>
        Started = 1,

        /// <summary>
        /// The watcher stop command was issued.
        /// </summary>
        Stopping = 2,

        /// <summary>
        /// The watcher is stopped.
        /// </summary>
        Stopped = 3,

        /// <summary>
        /// An error occurred during transition or scanning that stopped the watcher due
        /// to an error.
        /// </summary>
        Aborted = 4
    }
}