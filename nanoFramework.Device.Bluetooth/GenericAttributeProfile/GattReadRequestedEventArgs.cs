//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class contains the arguments for the StateChanged event.
    /// </summary>
    public sealed class GattReadRequestedEventArgs
    {
        private readonly ushort _descriptorId;
        private readonly ushort _eventId;
        private readonly GattSession _session = null;
        private readonly INativeDevice _nativeDevice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="session"></param>
        /// <param name="nativeDevice"></param>
        /// <param name="descriptorId"></param>
        public GattReadRequestedEventArgs(ushort descriptorId, ushort eventId, GattSession session, INativeDevice nativeDevice)
        {
            _descriptorId = descriptorId;
            _nativeDevice = nativeDevice;
            _eventId = eventId;
            _session = session;
        }

        /// <summary>
        /// Gets the GATT read request.
        /// </summary>
        /// <returns>Returns a GattReadRequest object.</returns>
        public GattReadRequest GetRequest()
        {
            return new GattReadRequest(_eventId, _nativeDevice);
        }
        
        internal ushort DescriptorId { get => _descriptorId; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        public GattSession Session { get => _session; }
    }
}
