//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This enumeration defines the advertisement status of a GattServiceProvider.
    /// </summary>
    public enum GattServiceProviderAdvertisementStatus
    {
        /// <summary>
        /// The GATT service was created.
        /// </summary>
        Created = 0,

        /// <summary>
        /// The GATT service is not advertising.
        /// </summary>
        Stopped = 1,

        /// <summary>
        /// The GATT service advertising has started.
        /// </summary>
        Started = 2,

        /// <summary>
        /// The GATT service was aborted.
        /// </summary>
        Aborted = 3,

        /// <summary>
        /// Indicates that the system was successfully able to issue the advertisement request,
        /// but not all of the requested data could be included in the advertisement.</summary>
        StartedWithoutAllAdvertisementData = 4
    }
}