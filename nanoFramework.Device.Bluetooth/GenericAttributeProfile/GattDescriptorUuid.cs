//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents an enumeration of the most well known Descriptor UUID values, and
    /// provides convenience methods for working with GATT descriptor UUIDs, and static
    /// properties providing descriptor UUIDs for common GATT descriptors.
    /// </summary>
    public static class GattDescriptorUuids
    {
        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Aggregate Format Descriptor UUID.
        /// </summary>
        public static Guid CharacteristicAggregateFormat { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.CharacteristicAggregateFormat); }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined Characteristic Extended Properties Descriptor UUID.
        /// </summary>
        public static Guid CharacteristicExtendedProperties { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.CharacteristicExtendedProperties); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic Presentation Format Descriptor
        /// </summary>
        public static Guid CharacteristicPresentationFormat { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.CharacteristicPresentationFormat); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Characteristic User Description Descriptor UUID.
        /// </summary>
        public static Guid CharacteristicUserDescription { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.CharacteristicUserDescription); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Client Characteristic Configuration Descriptor UUID.
        /// </summary>
        public static Guid ClientCharacteristicConfiguration { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.ClientCharacteristicConfiguration); }

        /// <summary>
        /// Gets the Bluetooth SIG-defined Server Characteristic Configuration Descriptor UUID.
        /// </summary>
        public static Guid ServerCharacteristicConfiguration { get => Utilities.CreateUuidFromShortCode((ushort)Utilities.GattNativeDescriptorUuid.ServerCharacteristicConfiguration); }
    }
}