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
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Heart Rate Measurement Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Heart Rate Measurement Characteristic UUID.
        //public static Guid HeartRateMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Battery Level Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Battery Level Characteristic UUID.
        //public static Guid BatteryLevel { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Blood Pressure Feature Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Blood Pressure Feature Characteristic UUID.
        //public static Guid BloodPressureFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Blood Pressure Measurement Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Blood Pressure Measurement Characteristic UUID.
        //public static Guid BloodPressureMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Body Sensor Location Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Body Sensor Location Characteristic UUID.
        //public static Guid BodySensorLocation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Csc Feature Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Csc Feature Characteristic UUID.
        //public static Guid CscFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Csc Measurement Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Csc Measurement Characteristic UUID.
        //public static Guid CscMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Glucose Feature Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Glucose Feature Characteristic UUID.
        //public static Guid GlucoseFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Glucose Measurement Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Glucose Measurement Characteristic UUID.
        //public static Guid GlucoseMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Glucose Measurement Context Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Glucose Measurement Context Characteristic UUID.
        //public static Guid GlucoseMeasurementContext { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Heart Rate Control Point Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Heart Rate Control Point Characteristic UUID.
        //public static Guid HeartRateControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Intermediate Cuff Pressure Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Intermediate Cuff Pressure Characteristic UUID.
        //public static Guid IntermediateCuffPressure { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Intermediate Temperature Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Intermediate Temperature Characteristic UUID.
        //public static Guid IntermediateTemperature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Measurement Interval Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Measurement Interval Characteristic UUID.
        //public static Guid MeasurementInterval { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Record Access Control Point Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Record Access Control Point Characteristic UUID.
        //public static Guid RecordAccessControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Rsc Feature Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Rsc Feature Characteristic UUID.
        //public static Guid RscFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Rsc Measurement Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Rsc Measurement Characteristic UUID.
        //public static Guid RscMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SC Control Point Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SC Control Point Characteristic UUID.
        //public static Guid SCControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Sensor Location Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Sensor Location Characteristic UUID.
        //public static Guid SensorLocation { get; }


        /// <summary>
        ///  Gets the Bluetooth SIG-defined Temperature Measurement Characteristic UUID.
        /// </summary>
        public static Guid TemperatureMeasurement { get => Utilities.CreateShortUuid("2a1c"); }

        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Temperature Type Characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Temperature Type Characteristic UUID.
        //public static Guid TemperatureType { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined GapPeripheralPreferredConnectionParameters characteristic
        ////     UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined GapPeripheralPreferredConnectionParameters characteristic
        ////     UUID.
        //public static Guid GapPeripheralPreferredConnectionParameters { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth GapPeripheralPrivacyFlag characterisitc UUID.
        ////
        //// Returns:
        ////     The Bluetooth GapPeripheralPrivacyFlag characteristic UUID.
        //public static Guid GapPeripheralPrivacyFlag { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined GapReconnectionAddress characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined GapReconnectionAddress characteristic UUID.
        //public static Guid GapReconnectionAddress { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined GattServiceChanged characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined GattServiceChanged characteristic UUID.
        //public static Guid GattServiceChanged { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined HardwareRevisionString characterisitc UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined HardwareRevisionString characteristic UUID.
        //public static Guid HardwareRevisionString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined HidControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined HidControlPoint characteristic UUID.
        //public static Guid HidControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined HidInformation characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined HidInformation characteristic UUID.
        //public static Guid HidInformation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Ieee1107320601RegulatoryCertificationDataList
        ////     characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Ieee1107320601RegulatoryCertificationDataList characteristic
        ////     UUID.
        //public static Guid Ieee1107320601RegulatoryCertificationDataList { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LnControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LnControlPoint characteristic UUID.
        //public static Guid LnControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LnFeature characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LnFeature characteristic UUID.
        //public static Guid LnFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LocalTimeInformation characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LocalTimeInformation characteristic UUID.
        //public static Guid LocalTimeInformation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LocationAndSpeed characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LocationAndSpeed characteristic UUID.
        //public static Guid LocationAndSpeed { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ManufacturerNameString characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ManufacturerNameString characteristic UUID.
        //public static Guid ManufacturerNameString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ModelNumberString UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ModelNumberString UUID.
        //public static Guid ModelNumberString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Navigation characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Navigation characteristic UUID.
        //public static Guid Navigation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined NewAlert characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined NewAlert characteristic UUID.
        //public static Guid NewAlert { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined PnpId characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined PnpId characteristic UUID.
        //public static Guid PnpId { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined PositionQuality characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined PositionQuality characteristic UUID.
        //public static Guid PositionQuality { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ProtocolMode characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ProtocolMode characteristic UUID.
        //public static Guid ProtocolMode { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CyclingPowerFeature characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CyclingPowerFeature characterisitc UUID.
        //public static Guid CyclingPowerFeature { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Report characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Report characteristic UUID.
        //public static Guid Report { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ReportMap characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ReportMap characteristic UUID.
        //public static Guid ReportMap { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined RingerControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined RingerControlPoint characteristic UUID.
        //public static Guid RingerControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined RingerSetting characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined RingerSetting characteristic UUID.
        //public static Guid RingerSetting { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ScanIntervalWindow characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ScanIntervalWindow characteristic UUID.
        //public static Guid ScanIntervalWindow { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ScanRefresh characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ScanRefresh characteristic UUID.
        //public static Guid ScanRefresh { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SerialNumberString characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SerialNumberString characteristic UUID.
        //public static Guid SerialNumberString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SoftwareRevisionString characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SoftwareRevisionString characteristic UUID.
        //public static Guid SoftwareRevisionString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SupportUnreadAlertCategory characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SupportUnreadAlertCategory characteristic UUID.
        //public static Guid SupportUnreadAlertCategory { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SupportedNewAlertCategory characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SupportedNewAlertCategory characteristic UUID.
        //public static Guid SupportedNewAlertCategory { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined SystemId characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined SystemId characteristic UUID.
        //public static Guid SystemId { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeAccuracy characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeAccuracy characteristic UUID.
        //public static Guid TimeAccuracy { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeSource characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeSource characteristic UUID.
        //public static Guid TimeSource { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeUpdateControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeUpdateControlPoint characteristic UUID.
        //public static Guid TimeUpdateControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeUpdateState characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeUpdateState characteristic UUID.
        //public static Guid TimeUpdateState { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeWithDst characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeWithDst characteristic UUID.
        //public static Guid TimeWithDst { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TimeZone characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TimeZone characteristic UUID.
        //public static Guid TimeZone { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TxPowerLevel characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TxPowerLevel characteristic UUID.
        //public static Guid TxPowerLevel { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined UnreadAlertStatus characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined UnreadAlertStatus characteristic UUID.
        //public static Guid UnreadAlertStatus { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined AlertCategoryId characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AlertCategoryId characteristic UUID.
        //public static Guid AlertCategoryId { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-Defined AlertCategoryIdBitMask characteristic UUID. To
        ////     view a list of all Bluetooth SIG-defined characteristic UUIDs, see Bluetooth
        ////     SIG-defined Characteristic UUIDs.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AlertCategoryIdBitMask characteristic UUID.
        //public static Guid AlertCategoryIdBitMask { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined AlertLevel characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AlertLevel characteristic UUID.
        //public static Guid AlertLevel { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined AlertNotificationControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AlertNotificationControlPoint characteristic UUID.
        //public static Guid AlertNotificationControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined AlertStatus characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AllertStatus characteristic UUID.
        //public static Guid AlertStatus { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined BootKeyboardInputReport characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined BootKayboardInputReport characteristic UUID.
        //public static Guid BootKeyboardInputReport { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined BootKeyboardOutputReport characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined characteristic UUID.
        //public static Guid BootKeyboardOutputReport { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined BootMouseInputReport characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined BootMouseInputReport characteristic UUID.
        //public static Guid BootMouseInputReport { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CurrentTime characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CurrentTime characteristic UUID.
        //public static Guid CurrentTime { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CyclingPowerControlPoint characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CyclingPowerControlPoint characteristic UUID.
        //public static Guid CyclingPowerControlPoint { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ReferenceTimeInformation characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ReferenceTimeInformation characteristic UUID.
        //public static Guid ReferenceTimeInformation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CyclingPowerMeasurement characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CyclingPowerMeasurement characteristic UUID.
        //public static Guid CyclingPowerMeasurement { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CyclingPowerVector characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CyclingPowerVector characteristic UUID.
        //public static Guid CyclingPowerVector { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined DateTime characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined DateTime characteristic UUID.
        //public static Guid DateTime { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined DayDateTime characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined DayDateTime characteristic UUID.
        //public static Guid DayDateTime { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined DayOfWeek characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined DayOfWeek characteristic UUID.
        //public static Guid DayOfWeek { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined DstOffset characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined DstOffset characteristic UUID.
        //public static Guid DstOffset { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ExactTime256 characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ExactTime256 characteristic UUID.
        //public static Guid ExactTime256 { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined FirmwareRevisionString characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined FirmwareRevisionString characteristic UUID.
        //public static Guid FirmwareRevisionString { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined GapAppearance characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined GapAppearance characteristic UUID.
        //public static Guid GapAppearance { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined GapDeviceName characteristic UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined GapDeviceName characteristic UUID.
        //public static Guid GapDeviceName { get; }
    }
}
