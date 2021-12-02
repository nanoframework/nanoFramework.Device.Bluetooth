//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the desired security level.
    /// </summary>
    public enum GattProtectionLevel
    {
        /// <summary>
        /// Uses the default protection level.
        /// </summary>
        Plain = 0,

        /// <summary>
        /// Require the link to be authenticated.
        /// </summary>
        AuthenticationRequired = 1,

        /// <summary>
        /// Require the link to be encrypted.
        /// </summary>
        EncryptionRequired = 2,

        /// <summary>
        /// Require the link to be encrypted and authenticated.
        /// </summary>
        EncryptionAndAuthenticationRequired = 3
    }
}