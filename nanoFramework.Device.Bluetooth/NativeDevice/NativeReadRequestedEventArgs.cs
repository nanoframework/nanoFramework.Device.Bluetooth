using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// Args to be constructed for when a OnReadRequested event is invoked.
    /// </summary>
    public sealed class NativeReadRequestedEventArgs : EventArgs
    {
        private readonly ushort _eventId;
        private readonly ushort _descriptorId;
        private readonly GattLocalCharacteristic _localCharacteristic;

        /// <summary>
        /// Creates new NativeReadRequestedEventsArgs.
        /// </summary>
        /// <param name="eventId">an id to associate a subsequent read request with</param>
        /// <param name="descriptorId">the id of the specific descriptor to be read (can be 0 to indicate reading the characteristic)</param>
        /// <param name="localCharacteristic">the characteristic to be read</param>
        public NativeReadRequestedEventArgs(ushort eventId, ushort descriptorId, GattLocalCharacteristic localCharacteristic)
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
