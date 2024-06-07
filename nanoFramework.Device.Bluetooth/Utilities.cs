//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//
using System;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Bluetooth library utilities.
    /// </summary>
    public static class Utilities
    {
        // UUID is the name used by Bluetooth SIG for a Guid
        // This value is the base UUID for all standard Bluetooth SIG UUIDs.
        private static byte[] baseUuid = new Guid("00000000-0000-1000-8000-00805f9b34fb").ToByteArray();

        /// <summary>
        /// Type of UUID.
        /// </summary>
        public enum UuidType
        {
            /// <summary>
            /// 16 bit UUID.
            /// </summary>
            Uuid16,
            /// <summary>
            /// 32 bit UUID.
            /// </summary>
            Uuid32,
            /// <summary>
            /// 128 bit UUID
            /// </summary>
            Uuid128
        };

        /// <summary>
        /// This enum assists in finding a string representation of a BT SIG assigned value for Descriptor UUIDs
        /// Reference: https://developer.bluetooth.org/gatt/descriptors/Pages/DescriptorsHomePage.aspx
        /// </summary>
        public enum GattNativeDescriptorUuid : ushort
        {
            /// <summary>
            /// CharacteristicExtendedProperties
            /// </summary>
            CharacteristicExtendedProperties = 0x2900,

            /// <summary>
            /// CharacteristicUserDescription
            /// </summary>
            CharacteristicUserDescription = 0x2901,

            /// <summary>
            /// ClientCharacteristicConfiguration
            /// </summary>
            ClientCharacteristicConfiguration = 0x2902,

            /// <summary>
            /// ServerCharacteristicConfiguration
            /// </summary>
            ServerCharacteristicConfiguration = 0x2903,

            /// <summary>
            /// CharacteristicPresentationFormat
            /// </summary>
            CharacteristicPresentationFormat = 0x2904,

            /// <summary>
            /// CharacteristicAggregateFormat
            /// </summary>
            CharacteristicAggregateFormat = 0x2905,

            /// <summary>
            /// ValidRange
            /// </summary>
            ValidRange = 0x2906,

            /// <summary>
            /// ExternalReportReference
            /// </summary>
            ExternalReportReference = 0x2907,

            /// <summary>
            /// ReportReference
            /// </summary>
            ReportReference = 0x2908,

            /// <summary>
            /// Number of Digitals 
            /// </summary>
            NumberDigitals = 0x2909,

            /// <summary>
            /// Value trigger settings
            /// </summary>
            ValueTriggerSetting = 0x290A,

            /// <summary>
            /// Environmental Sensing Configuration
            /// </summary>
            EssConfiguration = 0x290B,

            /// <summary>
            /// Environmental Sensing Measurement
            /// </summary>
            EssMeasurement = 0x290C,

            /// <summary>
            /// Environmental Sensing Trigger Setting
            /// </summary>
            EssTriggerSetting = 0x290D,

            /// <summary>
            /// Time Trigger Setting 
            /// </summary>
            TimeTriggerSetting = 0x290E
        }

        /// <summary>
        ///  Converts from standard 128bit UUID to the assigned 16bit UUID. Makes it easy to compare services
        ///  that devices expose to the standard list.
        /// </summary>
        /// <param name="uuid">UUID to convert to 16 bit short code Uuid</param>
        /// <returns>16bit UUID as ushort value.</returns>
        public static ushort ConvertUuidToShortId(Guid uuid)
        {
            // Get the short Uuid
            var bytes = uuid.ToByteArray();
            var shortUuid = (ushort)(bytes[0] | (bytes[1] << 8));
            return shortUuid;
        }

        /// <summary>
        ///  Converts from standard 128bit UUID to the assigned 32bit UUID. 
        /// </summary>
        /// <param name="uuid">UUID to convert to 32 bit short code Uuid.</param>
        /// <returns>32bit UUID as uint value.</returns>
        public static UInt32 ConvertUuidToIntId(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();
            return (UInt32)((bytes[2] << 24) + (bytes[3] << 16) + (bytes[0] << 8) + bytes[1]);
        }

        /// <summary>
        /// Create a Uuid/Guid from a short Bluetooth SIG short UUID
        /// </summary>
        /// <param name="uuid16">Bluetooth Short code UUID</param>
        /// <returns>A Guid using Bluetooth SIG UUID</returns>
        public static Guid CreateUuidFromShortCode(ushort uuid16)
        {
            byte[] bytes = new byte[16];
            baseUuid.CopyTo(bytes, 0);

            bytes[0] = (byte)(0x00ff & uuid16);
            bytes[1] = (byte)(uuid16 >> 8);

            return new Guid(bytes);
        }

        /// <summary>
        /// Is the GUID a Bluetooth SIG UUID, 16bit or 32bit.
        /// </summary>
        /// <param name="uuid">Guid with UUID value.</param>
        /// <returns>True if Bluetooth SIG UUID.</returns>
        public static bool IsBluetoothSigUUID(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();
            for (int index = 0; index < baseUuid.Length; index++)
            {
                if (baseUuid[index] != bytes[index])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Is passed Bluetooth SIG UUID a 16bit UUID format. 
        /// </summary>
        /// <param name="uuid">Guid with UUID Sig value.</param>
        /// <returns>True if Sig UUID is 16bit UUID</returns>
        public static bool IsBluetoothSigUUID16(Guid uuid)
        {
            byte[] bytes = uuid.ToByteArray();
            return (bytes[2] == 0) && (bytes[3] == 0);
        }

        /// <summary>
        /// Return the type of passed UUID.
        /// </summary>
        /// <param name="uuid">UUID to test.</param>
        /// <returns>UUID type.</returns>
        public static UuidType TypeOfUuid(Guid uuid)
        {
            if (!IsBluetoothSigUUID(uuid))
            {
                // 16 bit or 32 bit Bluetooth SIG UUID
                if (IsBluetoothSigUUID16(uuid))
                {
                    // 16bit UUID
                    return UuidType.Uuid16;
                }
                else
                {
                    // 32bit UUID
                    return UuidType.Uuid32;
                }
            }

            return UuidType.Uuid128;
        }
    }
}
