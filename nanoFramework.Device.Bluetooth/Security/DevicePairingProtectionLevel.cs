//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// The level of protection for pairing.
    /// </summary>
    public enum DevicePairingProtectionLevel
    {
        /// <summary>
        /// The default value. This should not be used.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Pair the device using no levels of protection.
        /// </summary>
        None = 1,

        /// <summary>
        /// Pair the device using encryption. 
        /// </summary>
        Encryption = 2,

        /// <summary>
        /// Pair the device using encryption and authentication.
        /// </summary>
        EncryptionAndAuthentication = 3
    }
}