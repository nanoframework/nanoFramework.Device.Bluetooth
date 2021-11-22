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
    /// See:- https://btprodspecificationrefs.blob.core.windows.net/assigned-values/16-bit%20UUID%20Numbers%20Document.pdf
    /// Current Service UUID 0x1800 to 0x181D
    /// </summary>
    public static class GattServiceUuids
    {
        /// <summary>
        ///  Gets the Bluetooth SIG-defined AlertNotification Service UUID.
        /// </summary>
        public static Guid AlertNotification { get => Utilities.CreateShortUuid("1811"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Automation IO Service UUID.
        /// </summary>
        public static Guid AutomationIO { get => Utilities.CreateShortUuid("1815"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Battery Service UUID.
        /// </summary>
        public static Guid Battery { get => Utilities.CreateShortUuid("180f"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Blood Pressure Service UUID.
        /// </summary>
        public static Guid BloodPressure { get => Utilities.CreateShortUuid("1810"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Body Composition Service UUID.
        /// </summary>
        public static Guid BodyComposition { get => Utilities.CreateShortUuid("181b"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined CurrentTime service UUID.
        /// </summary>
        public static Guid CurrentTime { get => Utilities.CreateShortUuid("1805"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined CyclingPower service UUID.
        /// </summary>
        public static Guid CyclingPower { get => Utilities.CreateShortUuid("1818"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Cycling Speed And Cadence Service UUID.
        /// </summary>
        public static Guid CyclingSpeedAndCadence { get => Utilities.CreateShortUuid("1816"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined DeviceInformation service UUID.
        /// </summary>
        public static Guid DeviceInformation { get => Utilities.CreateShortUuid("180a"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Environmental Sensing Service UUID.
        /// </summary>
        public static Guid EnvironmentalSensing { get => Utilities.CreateShortUuid("181a"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Access Service.
        /// </summary>
        public static Guid GenericAccess { get => Utilities.CreateShortUuid("1800"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined UUID for the Generic Attribute Service.
        /// </summary>
        public static Guid GenericAttribute { get => Utilities.CreateShortUuid("1801"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Glucose Service UUID.
        /// </summary>
        public static Guid Glucose { get => Utilities.CreateShortUuid("1808"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Health Thermometer Service UUID.
        /// </summary>
        public static Guid HealthThermometer { get => Utilities.CreateShortUuid("1809"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Heart Rate Service UUID.
        /// </summary>
        public static Guid HeartRate { get => Utilities.CreateShortUuid("180d"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined HumanInterfaceDevice service UUID.
        /// </summary>
        public static Guid HumanInterfaceDevice { get => Utilities.CreateShortUuid("1812"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined ImmediateAlert service UUID.
        /// </summary>
        public static Guid ImmediateAlert { get => Utilities.CreateShortUuid("1802"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined LinkLoss service UUID.
        /// </summary>
        public static Guid LinkLoss {get => Utilities.CreateShortUuid("1803"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined LocationAndNavigation service UUID.
        /// </summary>
        public static Guid LocationAndNavigation { get => Utilities.CreateShortUuid("1819"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined NextDstChange service UUID.
        /// </summary>
        public static Guid NextDstChange { get => Utilities.CreateShortUuid("1807"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined PhoneAlertStatus service UUID.
        /// </summary>
        public static Guid PhoneAlertStatus { get => Utilities.CreateShortUuid("180e"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined ReferenceTimeUpdate service UUID.
        /// </summary>
        public static Guid ReferenceTimeUpdate { get => Utilities.CreateShortUuid("1806"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Running Speed And Cadence Service UUID.
        /// </summary>
        public static Guid RunningSpeedAndCadence { get => Utilities.CreateShortUuid("1814"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined ScanParameters service UUID.
        /// </summary>
        public static Guid ScanParameters { get => Utilities.CreateShortUuid("1813"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined TxPower service UUID.
        /// </summary>
        public static Guid TxPower { get => Utilities.CreateShortUuid("1804"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined User Data service UUID.
        /// </summary>
        public static Guid UserData { get => Utilities.CreateShortUuid("181c"); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Weight Scale service UUID.
        /// </summary>
        public static Guid WeightScale { get => Utilities.CreateShortUuid("181d"); }
    }
}
