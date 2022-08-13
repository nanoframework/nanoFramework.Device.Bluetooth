//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  The result of local characteristic descriptor operations like CreateDescriptorAsync.
    /// </summary>
    public class GattLocalDescriptorResult 
    {
        private readonly BluetoothError _error;
        private readonly GattLocalDescriptor _descriptor;

        internal GattLocalDescriptorResult(GattLocalDescriptor descriptor, BluetoothError error)
        {
            _descriptor = descriptor;
            _error = error;
        }

        /// <summary>
        /// Gets the descriptor.
        /// </summary>
        public GattLocalDescriptor Descriptor { get => _descriptor; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public BluetoothError Error { get => _error; }
    }
}