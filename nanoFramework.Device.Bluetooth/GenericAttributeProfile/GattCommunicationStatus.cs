//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the return status of a GATT API related operation.
    /// </summary>
    public enum GattCommunicationStatus
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        Success = 0,

        /// <summary>
        /// No communication can be performed with the device, at this time.
        /// </summary>
        Unreachable = 1,

        /// <summary>
        /// There was a GATT communication protocol error.
        /// </summary>
        ProtocolError = 2,

        /// <summary>
        /// Access is denied.
        /// </summary>
        AccessDenied = 3
    }
}
