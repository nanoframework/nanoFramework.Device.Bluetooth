//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Bluetooth Address object
    /// </summary>
    public class BluetoothAddress
    {
        private ulong _address;
        private BluetoothAddressType _addressType;

        /// <summary>
        /// BluetoothAddress constructor
        /// </summary>
        /// <param name="Address">Bluetooth address </param>
        /// <param name="AddressType">Bluetooth Address type</param>
        public BluetoothAddress(ulong Address, BluetoothAddressType AddressType)
        {
            _address = Address;
            _addressType = AddressType;
        }

        /// <summary>
        /// Get Bluetouth address.
        /// </summary>
        public ulong Address { get { return _address; } }

        /// <summary>
        /// Get Bluetooth type.
        /// </summary>
        public BluetoothAddressType AddressType { get { return _addressType; } }
    }
}
