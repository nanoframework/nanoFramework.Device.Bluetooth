﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents the event arguments for WriteRequested.
    /// </summary>
    public sealed class GattWriteRequestedEventArgs //: IGattWriteRequestedEventArgs
    {
        private readonly ushort _eventID;
        private readonly GattSession _session = null;
        private readonly INativeDevice _nativeDevice;

        public GattWriteRequestedEventArgs(ushort eventID, GattSession session, INativeDevice nativeDevice)
        {
            _nativeDevice = nativeDevice;
            _eventID = eventID;
            _session = session;
        }

        /// <summary>
        /// Gets the write request.
        /// </summary>
        /// <returns>Returns a write request.</returns>
        public GattWriteRequest GetRequest()
        {
            return new GattWriteRequest(_eventID, _nativeDevice);
        }

        /// <summary>
        ///  Gets the session.
        /// </summary>
        public GattSession Session { get => _session; }
    }
}
