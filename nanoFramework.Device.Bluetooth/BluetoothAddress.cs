//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Bluetooth Address class.
    /// </summary>
    public class BluetoothAddress
    {
        private readonly ulong _address;
        private readonly BluetoothAddressType _addressType;

        /// <summary>
        /// BluetoothAddress constructor.
        /// </summary>
        /// <param name="Address">Bluetooth address.</param>
        /// <param name="AddressType">Bluetooth Address type.</param>
        public BluetoothAddress(ulong Address, BluetoothAddressType AddressType)
        {
            _address = Address;
            _addressType = AddressType;
        }

        /// <summary>
        /// Get Bluetooth address.
        /// </summary>
        public ulong Address { get => _address; }

        /// <summary>
        /// Gets Bluetooth type.
        /// </summary>
        public BluetoothAddressType AddressType { get => _addressType; }
    }
}
