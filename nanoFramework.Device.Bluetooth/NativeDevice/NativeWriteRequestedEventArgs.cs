﻿using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    public class NativeWriteRequestedEventArgs : EventArgs
    {
        public GattLocalCharacteristic LocalCharacteristic { get; }
        public ushort DescriptorId { get; }
        public ushort Id { get; }
    }
}
