//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Data type values used in BluetoothLEAdvertisementDataSection.
    /// </summary>
    public enum BluetoothLEAdvertisementDataSectionType
    {
        /// <summary>
        /// Section data type for the Bluetooth LE advertising interval.
        /// </summary>
        AdvertisingInterval = 0x1A,

        /// <summary>
        /// Section data type for the Bluetooth LE advertising appearance.
        /// </summary>
        Appearance = 0x19,

        /// <summary>
        /// Section data type for the Bluetooth LE complete local name.
        /// </summary>
        CompleteLocalName = 0x09,

        /// <summary>
        /// Section data type for the complete list of 128-bit Bluetooth LE service UUIDs.
        /// </summary>
        CompleteList128uuid = 0x07,

        /// <summary>
        /// Section data type for the complete list of 16-bit Bluetooth LE service UUIDs.
        /// </summary>
        CompleteList16uuid = 0x03,

        /// <summary>
        /// Section data type for the complete list of 32-bit Bluetooth LE service UUIDs.
        /// </summary>
        CompleteList32uuid = 0x05,

        /// <summary>
        /// Section data type for a set of flags for internal use.
        /// </summary>
        Flags = 0x01,

        /// <summary>
        /// Section data type for an incomplete list of 128-bit Bluetooth LE service UUIDs.
        /// </summary>
        IncompleteList128uuid = 0x06,

        /// <summary>
        /// Section data type for an incomplete list of 16-bit Bluetooth LE service UUIDs.
        /// </summary>
        IncompleteList16uuid = 0x02,

        /// <summary>
        /// Section data type for an incomplete list of 32-bit Bluetooth LE service UUIDs.
        /// </summary>
        IncompleteList32uuid = 0x04,

        /// <summary>
        /// Section data type for manufacturer-specific data for a Bluetooth LE advertisements.
        /// </summary>
        ManufacturerSpecificData = 0xFF,

        /// <summary>
        /// Section data type for the Peripheral connection interval range.
        /// </summary>
        PeripheralConnectionIntervalRange = 0x12,

        /// <summary>
        /// Section data type for a list of public Bluetooth LE target addresses.
        /// </summary>
        PublicTargetAddress = 0x17,

        /// <summary>
        /// Section data type for a list of random Bluetooth LE target addresses.
        /// </summary>
        RandomTargetAddress = 0x18,

        /// <summary>
        /// Section data type for service data for 128-bit Bluetooth LE UUIDs.
        /// </summary>
        ServiceData128bitUuid = 0x21,

        /// <summary>
        /// Section data type for service data for 16-bit Bluetooth LE UUIDs.
        /// </summary>
        ServiceData16bitUuid = 0x16,

        /// <summary>
        /// Section data type for service data for 32-bit Bluetooth LE UUIDs.
        /// </summary>
        ServiceData32bitUuid = 0x20,

        /// <summary>
        /// Section data type for a list of 128-bit Bluetooth LE service solicitation UUIDs.
        /// </summary>
        ServiceSolicitation128BitUuids = 0x15,

        /// <summary>
        /// Section data type for a list of 16-bit Bluetooth LE service solicitation UUIDs.
        /// </summary>
        ServiceSolicitation16BitUuids = 0x14,

        /// <summary>
        /// Section data type for a list of 32-bit Bluetooth LE service solicitation UUIDs.
        /// </summary>
        ServiceSolicitation32BitUuids = 0x1F,

        /// <summary>
        /// Section data type for a shortened local name.
        /// </summary>
        ShortenedLocalName = 0x08,

        /// <summary>
        /// Section data type for the Bluetooth LE transmit power level.
        /// </summary>
        TxPowerLevel = 0x0A
    }
}
