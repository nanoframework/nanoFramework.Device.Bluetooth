using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    public class NativeSubscribedClientsChangedEventArgs : EventArgs
    {
        public ushort Id { get; }
        public GattLocalCharacteristic LocalCharacteristic { get; }
    }
}
