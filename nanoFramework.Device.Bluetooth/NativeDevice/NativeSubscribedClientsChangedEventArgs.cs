using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NativeSubscribedClientsChangedEventArgs : EventArgs
    {
        private readonly ushort _id;
        private readonly GattLocalCharacteristic _localCharacteristic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="localCharacteristic"></param>
        public NativeSubscribedClientsChangedEventArgs(ushort eventId, GattLocalCharacteristic localCharacteristic)
        {
            _id = eventId;
            _localCharacteristic = localCharacteristic;
        }

        internal ushort Id { get { return _id; } }

        internal GattLocalCharacteristic LocalCharacteristic { get { return _localCharacteristic; } }
    }
}
