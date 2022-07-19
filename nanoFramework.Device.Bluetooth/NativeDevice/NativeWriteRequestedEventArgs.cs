using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NativeWriteRequestedEventArgs : EventArgs
    {
        private readonly ushort _id;
        private readonly ushort _descriptorId;
        private readonly GattLocalCharacteristic _localCharacteristic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="descriptorId"></param>
        /// <param name="localCharacteristic"></param>
        public NativeWriteRequestedEventArgs(ushort eventId, ushort descriptorId, GattLocalCharacteristic localCharacteristic)
        {
            _id = eventId;
            _descriptorId = descriptorId;
            _localCharacteristic = localCharacteristic;
        }

        internal ushort Id { get { return _id; } }

        internal ushort DescriptorId { get { return _descriptorId; } }

        internal GattLocalCharacteristic LocalCharacteristic { get { return _localCharacteristic; } }
    }
}
