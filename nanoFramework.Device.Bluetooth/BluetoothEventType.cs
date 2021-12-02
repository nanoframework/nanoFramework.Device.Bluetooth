//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Runtime.Events;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Event type for Bluetooth
    /// </summary>
    public enum BluetoothEventType
    {
        /// <summary>
        /// Attribute Read
        /// </summary>
        Read,

        /// <summary>
        /// Attribute write
        /// </summary>
        Write,

        /// <summary>
        /// Client Subscribed
        /// </summary>
        ClientSubscribed,

        /// <summary>
        /// Client unsubscribed or connection terminated
        /// </summary>
        ClientUnsubscribed
    }
}