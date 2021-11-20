//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  Indicates what type of write operation is to be performed.
    /// </summary>
    public enum GattWriteOption
    {
        /// <summary>
        /// The default GATT write procedure shall be used.
        /// </summary>
        WriteWithResponse = 0,

        /// <summary>
        ///  The Write Without Response procedure shall be used.
        /// </summary>
        WriteWithoutResponse = 1
    }
}
