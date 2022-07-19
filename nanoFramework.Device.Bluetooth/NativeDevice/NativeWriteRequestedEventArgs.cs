using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NativeWriteRequestedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="descriptorId"></param>
        /// <param name="localCharacteristic"></param>
        public NativeWriteRequestedEventArgs(ushort eventId, ushort descriptorId, GattLocalCharacteristic localCharacteristic)
        {
            Id = eventId;
            DescriptorId = descriptorId;
            LocalCharacteristic = localCharacteristic;
        }

        internal ushort Id { get; }
        internal ushort DescriptorId { get; }
        internal GattLocalCharacteristic LocalCharacteristic { get; }
    }
}
