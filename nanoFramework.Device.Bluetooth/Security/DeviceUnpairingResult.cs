//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Contains information about the result of attempting to unpair a device.
    /// </summary>
    public class DeviceUnpairingResult
    {
        private readonly DeviceUnpairingResultStatus _status;

        internal DeviceUnpairingResult(DeviceUnpairingResultStatus status)
        {
            _status = status;
        }


        /// <summary>
        /// Gets the paired status of the device after the pairing action completed.
        /// </summary>
        public DeviceUnpairingResultStatus Status => _status;
    }
}