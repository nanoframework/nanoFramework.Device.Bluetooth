using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// Args to be constructed for when a OnWriteRequested event is invoked.
    /// </summary>
    public sealed class NativeWriteRequestedEventArgs : EventArgs
    {
        private readonly ushort _eventId;
        private readonly ushort _descriptorId;
        private readonly GattLocalCharacteristic _localCharacteristic;

        /// <summary>
        /// Creates new NativeWriteRequestedEventArgs
        /// </summary>
        /// <param name="eventId">an id to associate a subsequent write request with</param>
        /// <param name="descriptorId">the id of the specific descriptor to be written to (can be 0 to indicate writing to the characteristic)</param>
        /// <param name="localCharacteristic">the characteristic to be written to</param>
        public NativeWriteRequestedEventArgs(ushort eventId, ushort descriptorId, GattLocalCharacteristic localCharacteristic)
        {
            _eventId = eventId;
            _descriptorId = descriptorId;
            _localCharacteristic = localCharacteristic;
        }

        internal ushort EventId { get { return _eventId; } }

        internal ushort DescriptorId { get { return _descriptorId; } }

        internal GattLocalCharacteristic LocalCharacteristic { get { return _localCharacteristic; } }
    }
}
