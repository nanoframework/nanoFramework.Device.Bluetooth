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
        private readonly ushort _eventID;
        private readonly GattSession _session = null;

        internal GattReadRequestedEventArgs(ushort eventID, GattSession session)
        {
            _eventID = eventID;
            _session = session;
        }

        /// <summary>
        /// Gets the GATT read request.
        /// </summary>
        /// <returns>Returns a GattReadRequest object.</returns>
        public GattReadRequest GetRequest()
        {
            return new GattReadRequest(_eventID);
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
