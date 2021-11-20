//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents an enumeration of the most well known Service UUID values, and provides
    /// convenience methods for working with GATT service UUIDs, and static properties
    /// providing service UUIDs for common GATT services. To view a list of all Bluetooth
    /// SIG-defined service UUIDs, see Bluetooth SIG-defined Service UUIDs.
    /// </summary>
    public static class GattServiceUuids
    {
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Battery Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Battery Service UUID.
        //public static Guid Battery { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Blood Pressure Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Blood Pressure Service UUID.
        //public static Guid BloodPressure { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Cycling Speed And Cadence Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Cycling Speed And Cadence Service UUID.
        //public static Guid CyclingSpeedAndCadence { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined UUID for the Generic Access Service.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined UUID for the Generic Access Service.
        //public static Guid GenericAccess { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined UUID for the Generic Attribute Service.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined UUID for the Generic Attribute Service.
        //public static Guid GenericAttribute { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Glucose Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Glucose Service UUID.
        //public static Guid Glucose { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Health Thermometer Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Health Thermometer Service UUID.
        //public static Guid HealthThermometer { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Heart Rate Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Heart Rate Service UUID.
        //public static Guid HeartRate { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined Running Speed And Cadence Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined Running Speed And Cadence Service UUID.
        //public static Guid RunningSpeedAndCadence { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined AlertNotification Service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined AlertNotification Service UUID.
        //public static Guid AlertNotification { get; }

        /// <summary>
        /// Gets the Bluetooth SIG-defined CurrentTime service UUID.
        /// </summary>
        public static Guid CurrentTime { get => Utilities.CreateShortUuid("1805"); }

        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined CyclingPower service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined CyclingPower service UUID.
        //public static Guid CyclingPower { get; }

        /// <summary>
        /// Gets the Bluetooth SIG-defined DeviceInformation service UUID.
        /// </summary>
        public static Guid DeviceInformation { get => Utilities.CreateShortUuid("180a"); }

        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined HumanInterfaceDevice service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined HumanInterfaceDevice service UUID.
        //public static Guid HumanInterfaceDevice { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ImmediateAlert service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ImmediateAlert service UUID.
        //public static Guid ImmediateAlert { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LinkLoss service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LinkLoss service UUID.
        //public static Guid LinkLoss { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined LocationAndNavigation service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined LocationAndNavigation service UUID.
        //public static Guid LocationAndNavigation { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined NextDstChange service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined NextDstChange service UUID.
        //public static Guid NextDstChange { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined PhoneAlertStatus service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined PhoneAlertStatus service UUID.
        //public static Guid PhoneAlertStatus { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ReferenceTimeUpdate service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ReferenceTimeUpdate service UUID.
        //public static Guid ReferenceTimeUpdate { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined ScanParameters service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined ScanParameters service UUID.
        //public static Guid ScanParameters { get; }
        ////
        //// Summary:
        ////     Gets the Bluetooth SIG-defined TxPower service UUID.
        ////
        //// Returns:
        ////     The Bluetooth SIG-defined TxPower service UUID.
        //public static Guid TxPower { get; }
    }
}
