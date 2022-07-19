using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NativeSubscribedClientsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="localCharacteristic"></param>
        public NativeSubscribedClientsChangedEventArgs(ushort eventId, GattLocalCharacteristic localCharacteristic)
        {
            Id = eventId;
            LocalCharacteristic = localCharacteristic;
        }
        
        internal ushort Id { get; }
        internal GattLocalCharacteristic LocalCharacteristic { get; }
    }
}
