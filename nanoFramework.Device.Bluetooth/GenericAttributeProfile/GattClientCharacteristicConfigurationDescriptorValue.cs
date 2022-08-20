//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the value of the GATT ClientCharacteristicConfigurationDescriptor.
    /// </summary>
    public enum GattClientCharacteristicConfigurationDescriptorValue
    {
        /// <summary>
        /// Neither notification nor indications are enabled.
        /// </summary>
        None = 0,

        /// <summary>
        /// Characteristic notifications are enabled.
        /// </summary>
        Notify = 1,

        /// <summary>
        ///  Characteristic indications are enabled.
        /// </summary>
        Indicate = 2
    }
}

