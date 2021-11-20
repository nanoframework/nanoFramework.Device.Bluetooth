//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class contains byte values for GATT protocol errors.
    /// </summary>
    public static class GattProtocolError
    {
        /// <summary>
        ///  Gets the byte value for an attribute not found error.
        /// </summary>
        public static byte AttributeNotFound { get => 10; }

        /// <summary>
        ///  Gets the byte value for an attribute not long error.
        /// </summary>
        public static byte AttributeNotLong { get => 11; }

        /// <summary>
        /// Gets the byte value for an insufficient authentication error.
        /// </summary>
        public static byte InsufficientAuthentication { get => 5; }

        /// <summary>
        /// Gets the byte value for an insufficient authorization error.
        /// </summary>
        public static byte InsufficientAuthorization { get => 8; }

        /// <summary>
        /// Gets the byte value for an insufficient encryption error.
        /// </summary>
        public static byte InsufficientEncryption { get => 15; }

        /// <summary>
        /// Gets the byte value for an insufficient encryption key size error.
        /// </summary>
        public static byte InsufficientEncryptionKeySize { get => 12; }

        /// <summary>
        ///  Gets the byte value for an insufficient resources error.
        /// </summary>
        public static byte InsufficientResources { get => 17; }

        /// <summary>
        ///  Gets the byte value for an invalid attribute value length error.
        /// </summary>
        public static byte InvalidAttributeValueLength { get => 13; }

        /// <summary>
        /// Gets the byte value for an invalid handle error.
        /// </summary>
        public static byte InvalidHandle { get => 1; }

        /// <summary>
        ///  Gets the byte value for an invalid offset error.
        /// </summary>
        public static byte InvalidOffset { get => 7; }

        /// <summary>
        /// Gets the byte value for an invalid PDU error.
        /// </summary>
        public static byte InvalidPdu { get => 4; }

        /// <summary>
        /// Gets the byte value for a prepare queue full error.
        /// </summary>
        public static byte PrepareQueueFull { get => 9; }

        /// <summary>
        ///  Gets the byte value for a read not permitted error.
        /// </summary>
        public static byte ReadNotPermitted { get => 2; }

        /// <summary>
        /// Gets the byte value for a request not supported error.
        /// </summary>
        public static byte RequestNotSupported { get => 6; }

        /// <summary>
        ///  Gets the byte value for an unlikely error.
        /// </summary>
        public static byte UnlikelyError { get => 14; }

        /// <summary>
        ///  Gets the byte value for an unsupported group type error.
        /// </summary>
        public static byte UnsupportedGroupType { get => 15; }

        /// <summary>
        ///  Gets the byte value for a write not permitted error.
        /// </summary>
        public static byte WriteNotPermitted { get => 3; }
    }
}

