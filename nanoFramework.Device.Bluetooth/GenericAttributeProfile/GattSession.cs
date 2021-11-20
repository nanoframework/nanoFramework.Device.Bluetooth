//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a GATT session.
    /// </summary>
    public sealed class GattSession :  IDisposable 
    {
        private readonly BluetoothDeviceId  _deviceId;

        /// <summary>
        /// Dispose GattSession object
        /// </summary>
        public void Dispose()
        { 
            // not used yet
        }

        /// <summary>
        /// Creates a new GattSession object from the specified deviceId.
        /// </summary>
        /// <param name="deviceId">The deviceId.</param>
        /// <returns> A new GattSession object.</returns>
        public static GattSession FromDeviceId(BluetoothDeviceId deviceId)
        {
            return new GattSession(deviceId);
        }

        internal GattSession(BluetoothDeviceId deviceId)
        {
            _deviceId = deviceId;
        }

        /// <summary>
        /// Gets the device ID.
        /// </summary>
        public BluetoothDeviceId DeviceId { get => _deviceId; }
    }
}
