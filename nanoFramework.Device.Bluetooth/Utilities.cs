//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//
using System;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Bluetooth library utilities
    /// </summary>
    public static class Utilities
    {
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
        /// <returns></returns>
        public static ushort ConvertUuidToShortId(Guid uuid)
        {
            // Get the short Uuid
            var bytes = uuid.ToByteArray();
            var shortUuid = (ushort)(bytes[0] | (bytes[1] << 8));
            return shortUuid;
        }

        /// <summary>
        /// Create a Uuid/Guid from a short Bluetooth SIG short UUID
        /// </summary>
        /// <param name="uuid16">Bluetooth Short code UUID</param>
        /// <returns>A Guid using Bluetooth SIG UUID</returns>
        public static Guid CreateUuidFromShortCode(ushort uuid16)
        {
            // UUID is the name used by Bluetooth SIG for a Guid
            // This is the base UUID for all standard Bluetooth SIG UUIDs 
            const string baseUuid = "00000000-0000-1000-8000-00805f9b34fb";

            Guid temp = new Guid(baseUuid);

            byte[] bytes = temp.ToByteArray();

            bytes[0] = (byte)(0x00ff & uuid16);
            bytes[1] = (byte)(uuid16 >> 8);

            return new Guid(bytes);
        }
    }
}
