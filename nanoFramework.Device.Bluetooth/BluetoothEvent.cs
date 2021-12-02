//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Runtime.Events;

namespace nanoFramework.Device.Bluetooth 
{
    internal class BluetoothEvent : BaseEvent
    {
        /// <summary>
        /// Type of Bluetooth event
        /// </summary>
        public BluetoothEventType type;

        /// <summary>
        /// Event or Connect ID
        /// </summary>
        public ushort ID;

        /// <summary>
        /// ID of Characteristic
        /// </summary>
        public ushort characteristicId;

        /// <summary>
        /// ID of Descriptor
        /// </summary>
        public ushort descriptorId;
    }
}
