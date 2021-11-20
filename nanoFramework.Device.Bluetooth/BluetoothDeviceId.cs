//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth device ID.
    /// </summary>
    public sealed class BluetoothDeviceId 
    {
        private readonly int _id;

        /// <summary>
        /// Creates a BluetoothDeviceId object from the device ID.
        /// </summary>
        /// <param name="deviceId">The device ID.</param>
        /// <returns></returns>
        public static BluetoothDeviceId FromId(int deviceId)
        {
            return new BluetoothDeviceId(deviceId);
        }

        internal BluetoothDeviceId(int deviceId)
        {
            _id = deviceId;
        }

        /// <summary>
        /// Gets the Bluetooth device ID.
        /// </summary>
        public int Id { get => _id; }

        /// <summary>
        /// Gets a boolean indicating if this is a classic device.
        /// </summary>
        public bool IsClassicDevice { get => false; }

        /// <summary>
        /// Gets a boolean indicating if this is a LowEnergy device.
        /// </summary>
        public bool IsLowEnergyDevice { get => true; }
    }
}
