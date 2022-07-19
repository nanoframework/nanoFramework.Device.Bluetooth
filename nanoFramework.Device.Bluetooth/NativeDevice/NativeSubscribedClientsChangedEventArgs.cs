using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// Args to be constructed for when a OnClientSubscribed or OnClientUnsubscribed event is invoked.
    /// </summary>
    public sealed class NativeSubscribedClientsChangedEventArgs : EventArgs
    {
        private readonly ushort _deviceId;
        private readonly GattLocalCharacteristic _localCharacteristic;

        /// <summary>
        /// Creates new NativeSubscribedClientsChangedEventArgs.
        /// </summary>
        /// <param name="deviceId">the id of the device the event is associated with</param>
        /// <param name="localCharacteristic">the characteristic that is (un)subscribed to</param>
        public NativeSubscribedClientsChangedEventArgs(ushort deviceId, GattLocalCharacteristic localCharacteristic)
        {
            _deviceId = deviceId;
            _localCharacteristic = localCharacteristic;
        }

        internal ushort DeviceId { get { return _deviceId; } }

        internal GattLocalCharacteristic LocalCharacteristic { get { return _localCharacteristic; } }
    }
}
