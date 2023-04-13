//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Represents the possible states of the BluetoothLEAdvertisementPublisher.
    /// </summary>
    public enum BluetoothLEAdvertisementPublisherStatus
    {
        /// <summary>
        /// The initial status of the publisher.
        /// </summary>
        Created = 0,

        /// <summary>
        /// The publisher is waiting to get service time.
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// The publisher is being serviced and has started advertising.
        /// </summary>
        Started = 2,

        /// <summary>
        /// The publisher was issued a stop command.
        /// </summary>
        Stopping = 3,

        /// <summary>
        /// The publisher was issued a stop command.
        /// </summary>
        Stopped = 4,

        /// <summary>
        /// The publisher is aborted due to an error.
        /// </summary>
        Aborted = 5
    }
}