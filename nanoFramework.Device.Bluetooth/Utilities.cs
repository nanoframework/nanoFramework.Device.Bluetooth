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
            ReportReference = 0x2908
        }

        /// <summary>
        ///  Converts from standard 128bit UUID to the assigned 32bit UUIDs. Makes it easy to compare services
        ///  that devices expose to the standard list.
        /// </summary>
        /// <param name="uuid">UUID to convert to 32 bit</param>
        /// <returns></returns>
        public static ushort ConvertUuidToShortId(Guid uuid)
        {
            // Get the short Uuid
            var bytes = uuid.ToByteArray();
            var shortUuid = (ushort)(bytes[0] | (bytes[1] << 8));
            return shortUuid;
        }

        /// <summary>
        /// Converts from a buffer to a properly sized byte array
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ReadBufferToBytes(Buffer buffer)
        {
            var dataLength = buffer.Length;
            var data = new byte[dataLength];

            var reader = DataReader.FromBuffer(buffer);
            reader.ReadBytes(data);

            return data;
        }

        /// <summary>
        /// Create a Uuid from a short Bluetooth SIG short UUID
        /// </summary>
        /// <param name="uuid16">String of 4 digits of short UUID.</param>
        /// <returns></returns>
        public static Guid CreateShortUuid(string uuid16)
        {
            // UUID is the name used by Bluetooth SIG for a Guid
            // This is the base UUID for all standard Bluetooth SIG UUIDs 
            return new Guid("0000" + uuid16 + "-0000-1000-8000-00805f9b34fb");
        }
    }
}
