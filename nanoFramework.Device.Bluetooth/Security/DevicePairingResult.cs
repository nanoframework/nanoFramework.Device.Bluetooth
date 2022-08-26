//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Contains information about the result of attempting to pair a device.
    /// </summary>
    public class DevicePairingResult 
    {
        private readonly DevicePairingProtectionLevel _protectionLevelUsed;
        private readonly DevicePairingResultStatus _status;

        internal DevicePairingResult(DevicePairingProtectionLevel protectionLevelUsed, DevicePairingResultStatus status)
        {
            _protectionLevelUsed = protectionLevelUsed;
            _status = status;
        }

        /// <summary>
        /// Gets the level of protection used to pair the device.
        /// </summary>
        public DevicePairingProtectionLevel ProtectionLevelUsed { get => _protectionLevelUsed; }

         /// <summary>
        /// Gets the paired status of the device after the pairing action completed.
        /// </summary>
        public DevicePairingResultStatus Status { get => _status; }
    }
}