//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents an enumeration of the most well known Characteristic UUID values,
    /// and provides convenience methods for working with GATT characteristic UUIDs,
    /// and static properties providing characteristic UUIDs for common GATT characteristics.
    /// </summary>
    public static class GattCharacteristicUuids
    {
        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Measurement Characteristic UUID (0x2A37)
        /// </summary>
        public static Guid HeartRateMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A37); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Level Characteristic UUID (0x2A19).
        /// </summary>
        public static Guid BatteryLevel { get => Utilities.CreateUuidFromShortCode(0x2A19); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Feature Characteristic UUID (0x2A49).
        /// </summary>
        public static Guid BloodPressureFeature { get => Utilities.CreateUuidFromShortCode(0x2A49); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Blood Pressure Measurement Characteristic UUID ( 0x2A35).
        /// </summary>
        public static Guid BloodPressureMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A35); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Body Sensor Location Characteristic UUID (0x2A38).
        /// </summary>
        public static Guid BodySensorLocation { get => Utilities.CreateUuidFromShortCode(0x2A38); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Csc Feature Characteristic UUID (0x2A5C).
        /// </summary>
        public static Guid CscFeature { get => Utilities.CreateUuidFromShortCode(0x2A5C); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Csc Measurement Characteristic UUID (0x2A5B).
        /// </summary>
        public static Guid CscMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A5B); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Glucose Feature Characteristic UUID (0x2A51).
        /// </summary>
        public static Guid GlucoseFeature { get => Utilities.CreateUuidFromShortCode(0x2A51); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Measurement Characteristic UUID (0x2A18).
        /// </summary>
        public static Guid GlucoseMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A18); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Measurement Context Characteristic UUID (0x2A34).
        /// </summary>
        public static Guid GlucoseMeasurementContext { get => Utilities.CreateUuidFromShortCode(0x2A34); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Control Point Characteristic UUID (0x2A39).
        /// </summary>
        public static Guid HeartRateControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A39); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Intermediate Cuff Pressure Characteristic UUID (0x2A36).
        /// </summary>
        public static Guid IntermediateCuffPressure { get => Utilities.CreateUuidFromShortCode(0x2A36); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Intermediate Temperature Characteristic UUID (0x2A1E).
        /// </summary>
        public static Guid IntermediateTemperature { get => Utilities.CreateUuidFromShortCode(0x2A1E); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Measurement Interval Characteristic UUID (0x2A21).
        /// </summary>
        public static Guid MeasurementInterval { get => Utilities.CreateUuidFromShortCode(0x2A21); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Record Access Control Point Characteristic UUID (0x2A52).
        /// </summary>
        public static Guid RecordAccessControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A52); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Rsc Feature Characteristic UUID (0x2A54).
        /// </summary>
        public static Guid RscFeature { get => Utilities.CreateUuidFromShortCode(0x2A54); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Rsc Measurement Characteristic UUID (0x2A53).
        /// </summary>
        public static Guid RscMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A53); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined SC Control Point Characteristic UUID (0x2A55).
        /// </summary>
        public static Guid SCControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A55); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Sensor Location Characteristic UUID (0x2A5D).
        /// </summary>
        public static Guid SensorLocation { get => Utilities.CreateUuidFromShortCode(0x2A5D); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Temperature Measurement Characteristic UUID (0x2a1c).
        /// </summary>
        public static Guid TemperatureMeasurement { get => Utilities.CreateUuidFromShortCode(0x2a1c); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Temperature Type Characteristic UUID (0x2A1D).
        /// </summary>
        public static Guid TemperatureType { get => Utilities.CreateUuidFromShortCode(0x2A1D); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Gap Peripheral Preferred Connection Parameters characteristic UUID (0x2A04).
        /// </summary>
        public static Guid GapPeripheralPreferredConnectionParameters { get => Utilities.CreateUuidFromShortCode(0x2A04); }

        /// <summary>
        /// Gets the Bluetooth Gap Peripheral PrivacyFlag characteristic UUID (0x2A02).
        /// </summary>
        public static Guid GapPeripheralPrivacyFlag { get => Utilities.CreateUuidFromShortCode(0x2A02); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Gap Reconnection Address characteristic UUID (0x2A03).
        /// </summary>
        public static Guid GapReconnectionAddress { get => Utilities.CreateUuidFromShortCode(0x2A03); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Gatt Service Changed characteristic UUID (0x2A05).
        /// </summary>
        public static Guid GattServiceChanged { get => Utilities.CreateUuidFromShortCode(0x2A05); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Hardware Revision String characteristic UUID (0x2A27).
        /// </summary>
        public static Guid HardwareRevisionString { get => Utilities.CreateUuidFromShortCode(0x2A27); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Hid Control Point characteristic UUID (0x2A4C).
        /// </summary>
        public static Guid HidControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A4C); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Hid Information characteristic UUID (0x2A4A).
        /// </summary>
        public static Guid HidInformation { get => Utilities.CreateUuidFromShortCode(0x2A4A); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Ieee 1107320601 Regulatory Certification Data List
        /// characteristic UUID (0x2A2A).
        /// </summary>
        public static Guid Ieee1107320601RegulatoryCertificationDataList { get => Utilities.CreateUuidFromShortCode(0x2A2A); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Ln Control Point characteristic UUID (0x2A6B).
        /// </summary>
        public static Guid LnControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A6B); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Ln Feature characteristic UUID (0x2A6A).
        /// </summary>
        public static Guid LnFeature { get => Utilities.CreateUuidFromShortCode(0x2A6A); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Local Time Information characteristic UUID (0x2A0F).
        /// </summary>
        public static Guid LocalTimeInformation { get => Utilities.CreateUuidFromShortCode(0x2A0F); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Location And Speed characteristic UUID (0x2A67).
        /// </summary>
        public static Guid LocationAndSpeed { get => Utilities.CreateUuidFromShortCode(0x2A67); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Manufacturer Name String characteristic UUID (0x2A29).
        /// </summary>
        public static Guid ManufacturerNameString { get => Utilities.CreateUuidFromShortCode(0x2A29); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Model Number String UUID (0x2A24).
        /// </summary>
        public static Guid ModelNumberString { get => Utilities.CreateUuidFromShortCode(0x2A24); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Navigation characteristic UUID (0x2A68).
        /// </summary>
        public static Guid Navigation { get => Utilities.CreateUuidFromShortCode(0x2A68); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined NewAlert characteristic UUID (0x2A46).
        /// </summary>
        public static Guid NewAlert { get => Utilities.CreateUuidFromShortCode(0x2A46); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined PnpId characteristic UUID (0x2A50).
        /// </summary>
        public static Guid PnpId { get => Utilities.CreateUuidFromShortCode(0x2A50); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Position Quality characteristic UUID (0x2A69).
        /// </summary>
        public static Guid PositionQuality { get => Utilities.CreateUuidFromShortCode(0x2A69); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Protocol Mode characteristic UUID (0x2A4E).
        /// </summary>
        public static Guid ProtocolMode { get => Utilities.CreateUuidFromShortCode(0x2A4E); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Feature characteristic UUID (0x2A65).
        /// </summary>
        public static Guid CyclingPowerFeature { get => Utilities.CreateUuidFromShortCode(0x2A65); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Report characteristic UUID (0x2A4D).
        /// </summary>
        public static Guid Report { get => Utilities.CreateUuidFromShortCode(0x2A4D); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Report Map characteristic UUID (0x2A4B).
        /// </summary>
        public static Guid ReportMap { get => Utilities.CreateUuidFromShortCode(0x2A4B); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Ringer Control Point characteristic UUID (0x2A40).
        /// </summary>
        public static Guid RingerControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A40); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Ringer Setting characteristic UUID (0x2A41).
        /// </summary>
        public static Guid RingerSetting { get => Utilities.CreateUuidFromShortCode(0x2A41); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined ScanIntervalWindow characteristic UUID (0x2A4F).
        /// </summary>
        public static Guid ScanIntervalWindow { get => Utilities.CreateUuidFromShortCode(0x2A4F); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Scan Refresh characteristic UUID (0x2A31).
        /// </summary>
        public static Guid ScanRefresh { get => Utilities.CreateUuidFromShortCode(0x2A31); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Serial Number String characteristic UUID (0x2A25).
        /// </summary>
        public static Guid SerialNumberString { get => Utilities.CreateUuidFromShortCode(0x2A25); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Software Revision String characteristic UUID (0x2A28).
        /// </summary>
        public static Guid SoftwareRevisionString { get => Utilities.CreateUuidFromShortCode(0x2A28); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Support Unread Alert Category characteristic UUID (0x2A48).
        /// </summary>
        public static Guid SupportUnreadAlertCategory { get => Utilities.CreateUuidFromShortCode(0x2A48); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined SupportedNewAlertCategory characteristic UUID (0x2A47).
        /// </summary>
        public static Guid SupportedNewAlertCategory { get => Utilities.CreateUuidFromShortCode(0x2A47); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined SystemId characteristic UUID (0x2A23).
        /// </summary>
        public static Guid SystemId { get => Utilities.CreateUuidFromShortCode(0x2A23); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Time Accuracy characteristic UUID (0x2A12).
        /// </summary>
        public static Guid TimeAccuracy { get => Utilities.CreateUuidFromShortCode(0x2A12); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined TimeSource characteristic UUID (0x2A13).
        /// </summary>
        public static Guid TimeSource { get => Utilities.CreateUuidFromShortCode(0x2A13); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Time Update Control Point characteristic UUID (0x2A16).
        /// </summary>
        public static Guid TimeUpdateControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A16); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Time Update State characteristic UUID (0x2A17).
        /// </summary>
        public static Guid TimeUpdateState { get => Utilities.CreateUuidFromShortCode(0x2A17); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Time With Dst characteristic UUID (0x2A11).
        /// </summary>
        public static Guid TimeWithDst { get => Utilities.CreateUuidFromShortCode(0x2A11); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined TimeZone characteristic UUID (0x2A0E).
        /// </summary>
        public static Guid TimeZone { get => Utilities.CreateUuidFromShortCode(0x2A0E); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Tx Power Level characteristic UUID (0x2A07).
        /// </summary>
        public static Guid TxPowerLevel { get => Utilities.CreateUuidFromShortCode(0x2A07); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Unread Alert Status characteristic UUID (0x2A45).
        /// </summary>
        public static Guid UnreadAlertStatus { get => Utilities.CreateUuidFromShortCode(0x2A45); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined AlertCategoryId characteristic UUID (0x2A43).
        /// </summary>
        public static Guid AlertCategoryId { get => Utilities.CreateUuidFromShortCode(0x2A43); }

        /// <summary>
        /// Gets the Bluetooth SIG-Defined AlertCategoryIdBitMask characteristic UUID. To
        /// view a list of all Bluetooth SIG-defined characteristic UUIDs, see Bluetooth
        /// SIG-defined Characteristic UUIDs (0x2A42).
        /// </summary>
        public static Guid AlertCategoryIdBitMask { get => Utilities.CreateUuidFromShortCode(0x2A42); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Level characteristic UUID (0x2A06).
        /// </summary>
        public static Guid AlertLevel { get => Utilities.CreateUuidFromShortCode(0x2A06); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Alert Notification ControlPoint characteristic UUID (0x2A44).
        /// </summary>
        public static Guid AlertNotificationControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A44); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined AlertStatus characteristic UUID (0x2A3F).
        /// </summary>
        public static Guid AlertStatus { get => Utilities.CreateUuidFromShortCode(0x2A3F); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Boot Keyboard Input Report characteristic UUID (0x2A22).
        /// </summary>
        public static Guid BootKeyboardInputReport { get => Utilities.CreateUuidFromShortCode(0x2A22); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined BootKeyboardOutputReport characteristic UUID (0x2A32).
        /// </summary>
        public static Guid BootKeyboardOutputReport { get => Utilities.CreateUuidFromShortCode(0x2A32); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined BootMouseInputReport characteristic UUID (0x2A33).
        /// </summary>
        public static Guid BootMouseInputReport { get => Utilities.CreateUuidFromShortCode(0x2A33); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Current Time characteristic UUID (0x2A2B).
        /// </summary>
        public static Guid CurrentTime { get => Utilities.CreateUuidFromShortCode(0x2A2B); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined CyclingPowerControlPoint characteristic UUID (0x2A66).
        /// </summary>
        public static Guid CyclingPowerControlPoint { get => Utilities.CreateUuidFromShortCode(0x2A66); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Reference Time Information characteristic UUID (0x2A14).
        /// </summary>
        public static Guid ReferenceTimeInformation { get => Utilities.CreateUuidFromShortCode(0x2A14); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Measurement characteristic UUID (0x2A63).
        /// </summary>
        public static Guid CyclingPowerMeasurement { get => Utilities.CreateUuidFromShortCode(0x2A63); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Power Vector characteristic UUID (0x2A64).
        /// </summary>
        public static Guid CyclingPowerVector { get => Utilities.CreateUuidFromShortCode(0x2A64); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Date Time characteristic UUID (0x2A08).
        /// </summary>
        public static Guid DateTime { get => Utilities.CreateUuidFromShortCode(0x2A08); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Day Date Time characteristic UUID (0x2A0A).
        /// </summary>
        public static Guid DayDateTime { get => Utilities.CreateUuidFromShortCode(0x2A0A); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined DayOfWeek characteristic UUID (0x2A09).
        /// </summary>
        public static Guid DayOfWeek { get => Utilities.CreateUuidFromShortCode(0x2A09); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined DstOffset characteristic UUID (0x2A0D).
        /// </summary>
        public static Guid DstOffset { get => Utilities.CreateUuidFromShortCode(0x2A0D); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined ExactTime256 characteristic UUID (0x2A0C).
        /// </summary>
        public static Guid ExactTime256 { get => Utilities.CreateUuidFromShortCode(0x2A0C); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Firmware Revision String characteristic UUID (0x2A26).
        /// </summary>
        public static Guid FirmwareRevisionString { get => Utilities.CreateUuidFromShortCode(0x2A26); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Gap Appearance characteristic UUID (0x2A01).
        /// </summary>
        public static Guid GapAppearance { get => Utilities.CreateUuidFromShortCode(0x2A01); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Gap DeviceName characteristic UUID (0x2A00).
        /// </summary>
        public static Guid GapDeviceName { get => Utilities.CreateUuidFromShortCode(0x2A00); }
    }
}
