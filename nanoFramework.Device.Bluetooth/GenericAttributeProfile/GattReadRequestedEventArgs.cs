//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class contains the arguments for the StateChanged event.
    /// </summary>
    public sealed class GattReadRequestedEventArgs
    {
        private readonly ushort _eventId;
        private readonly GattSession _session;

        internal GattReadRequestedEventArgs(ushort eventId, GattSession session)
        {
            _eventId = eventId;
            _session = session;
        }

        /// <summary>
        /// Gets the GATT read request.
        /// </summary>
        /// <returns>Returns a GattReadRequest object.</returns>
        public GattReadRequest GetRequest()
        {
            return new GattReadRequest(_eventId);
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        public GattSession Session
        {
            get => _session;
        }
    }
}
