//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Runtime.Events;

namespace nanoFramework.Device.Bluetooth
{
    internal class BluetoothEventServer : BaseEvent
    {
        /// <summary>
        /// Type of Bluetooth event.
        /// </summary>
        public BluetoothEventType type;

        /// <summary>
        /// Event or Connect id.
        /// </summary>
        public ushort id;

        /// <summary>
        /// id of Characteristic.
        /// </summary>
        public ushort characteristicId;

        /// <summary>
        /// id of Descriptor.
        /// </summary>
        public ushort descriptorId;
    }

    internal class BluetoothEventScan : BaseEvent
    {
        /// <summary>
        /// Type of Bluetooth event.
        /// </summary>
        public BluetoothEventType type;

        /// <summary>
        /// Event id.
        /// </summary>
        public ushort id;
    }

    internal class BluetoothEventCentral : BaseEvent
    {
        /// <summary>
        /// Type of Bluetooth event.
        /// </summary>
        public BluetoothEventType type;

        /// <summary>
        /// Connection Handle.
        /// </summary>
        public ushort connectionHandle;

        /// <summary>
        /// status of event.
        /// </summary>
        public ushort status;

        /// <summary>
        /// Attribute Handle of service.
        /// </summary>
        public ushort serviceHandle;

        /// <summary>
        /// Attribute Handle of characteristic.
        /// </summary>
        public ushort characteristicHandle;
    }

    internal class BluetoothEventSesssion : BaseEvent
    {
        /// <summary>
        /// Type of Bluetooth event.
        /// </summary>
        public BluetoothEventType type;

        /// <summary>
        /// Connection Handle.
        /// </summary>
        public ushort connectionHandle;

        /// <summary>
        /// status of event.
        /// </summary>
        public ushort status;

        /// <summary>
        /// Any extra data.
        /// </summary>
        public ushort data;

        /// <summary>
        /// Used for when 32 bit data supplied (pin).
        /// </summary>
        public UInt32 data32;
    }
}
