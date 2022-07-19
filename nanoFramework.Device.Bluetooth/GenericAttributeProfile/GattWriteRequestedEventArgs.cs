//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents the event arguments for WriteRequested.
    /// </summary>
    public sealed class GattWriteRequestedEventArgs //: IGattWriteRequestedEventArgs
    {
        private readonly ushort _eventId;
        private readonly GattSession _session;

        internal GattWriteRequestedEventArgs(ushort eventId, GattSession session)
        {
            _eventId = eventId;
            _session = session;
        }

        /// <summary>
        /// Gets the write request.
        /// </summary>
        /// <returns>Returns a write request.</returns>
        public GattWriteRequest GetRequest()
        {
            return new GattWriteRequest(_eventId);
        }

        /// <summary>
        ///  Gets the session.
        /// </summary>
        public GattSession Session { get => _session; }
    }
}