﻿//
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
        private readonly INativeDevice _nativeDevice;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="session"></param>
        /// <param name="nativeDevice"></param>
        public GattReadRequestedEventArgs(ushort eventID, GattSession session, INativeDevice nativeDevice)
        {
            _nativeDevice = nativeDevice;
            _eventID = eventID;
            _session = session;
        }

        /// <summary>
        /// Gets the GATT read request.
        /// </summary>
        /// <returns>Returns a GattReadRequest object.</returns>
        public GattReadRequest GetRequest()
        {
            return new GattReadRequest(_eventID, _nativeDevice);
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        public GattSession Session { get => _session; }
    }
}