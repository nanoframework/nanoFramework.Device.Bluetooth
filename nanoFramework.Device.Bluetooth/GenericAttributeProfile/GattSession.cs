//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a GATT session.
    /// </summary>
    public class GattSession
    {
        private readonly BluetoothDeviceId _deviceId;
        private ushort _maxMtuSize;

        /// <summary>
        /// Delegate for SessionStatusChanged events.
        /// </summary>
        /// <param name="sender">GattSession sending event.</param>
        /// <param name="args">Event arguments.</param>
        public delegate void GattSessionStatusChangedEventHandler(Object sender, GattSessionStatusChangedEventArgs args);

        /// <summary>
        /// Session status change event.
        /// </summary>
        public event GattSessionStatusChangedEventHandler SessionStatusChanged;

        /// <summary>
        /// An event that is raised when the maximum protocol data unit (PDU) size changes. 
        /// The PDU is also known as the maximum transmission unit (MTU).
        /// </summary>
        public event EventHandler MaxPduSizeChanged; 

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

        internal void OnEvent(BluetoothEventSesssion btEvent)
        {
            //Debug.WriteLine($"# GattServiceProvider OnEvent, type:{btEvent.type} status:{btEvent.status}");

            switch (btEvent.type)
            {
                case BluetoothEventType.ClientConnected:
                    SessionStatusChanged?.Invoke(this, new GattSessionStatusChangedEventArgs(GattSessionStatus.Active, 0));
                    break;

                case BluetoothEventType.ClientDisconnected:
                    SessionStatusChanged?.Invoke(this, new GattSessionStatusChangedEventArgs(GattSessionStatus.Closed, 0));
                    break;

                case BluetoothEventType.ClientSessionChanged:
                    // Update max MTU size in GattSession
                    _maxMtuSize = btEvent.data;
                    MaxPduSizeChanged?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        /// <summary>
        /// Gets the device id.
        /// </summary>
        public BluetoothDeviceId DeviceId { get => _deviceId; }

        /// <summary>
        /// Gets the maximum transmission unit (MTU) size. 
        /// </summary>
        public ushort MaxMtuSize { get => _maxMtuSize; }

    }
}
