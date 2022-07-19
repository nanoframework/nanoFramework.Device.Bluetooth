using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth
{
    public class NativeReadRequestedEventArgs : EventArgs
    {
        public ushort Id { get; }
        public ushort DescriptorId { get;  }
        public GattLocalCharacteristic LocalCharacteristic { get;  }
    }
}
