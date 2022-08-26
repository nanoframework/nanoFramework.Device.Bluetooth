//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// The result of the unpairing action.
    /// </summary>
    public enum DeviceUnpairingResultStatus
    {
        /// <summary>
        /// The device object is successfully unpaired.
        /// </summary>
        Unpaired = 0,

        /// <summary>
        /// The device object is successfully unpaired.
        /// </summary>
        AlreadyUnpaired = 1,

        /// <summary>
        /// The device object is successfully unpaired.
        /// </summary>
        OperationAlreadyInProgress = 2,

        /// <summary>
        /// The caller does not have sufficient permissions to unpair the device.
        /// </summary>
        AccessDenied = 3,

        /// <summary>
        /// An unknown failure occurred.
        /// </summary>
        Failed = 4
    }
}
