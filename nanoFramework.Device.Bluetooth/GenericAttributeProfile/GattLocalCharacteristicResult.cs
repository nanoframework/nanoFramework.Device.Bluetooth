//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// A result of CreateCharacteristic
    /// </summary>
    public sealed class GattLocalCharacteristicResult
    {
        private readonly GattLocalCharacteristic _characteristic;
        private readonly BluetoothError _error;

        internal GattLocalCharacteristicResult(GattLocalCharacteristic Characteristic, BluetoothError Error)
        {
            _characteristic = Characteristic;
            _error = Error;
        }

        /// <summary>
        /// Gets the characteristic of the GATT service.
        /// </summary>
        public GattLocalCharacteristic Characteristic { get => _characteristic; }

        /// <summary>
        /// Gets the Bluetooth error.
        /// </summary>
        public BluetoothError Error { get => _error; }
    }
}