//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;

namespace nanoFramework.Device.Bluetooth.Spp
{
    /// <summary>
    /// Event argument for SPP receive data events
    /// </summary>
    public class SppReceivedDataEventArgs
    {
        private readonly byte[] _data;

        internal SppReceivedDataEventArgs(byte[] data)
        {
            _data = data;
        }

        /// <summary>
        /// Received data as byte[].
        /// </summary>
        public byte[] DataBytes { get => _data; }

        /// <summary>
        /// Received data as string.
        /// </summary>
        public String DataString { get => Encoding.UTF8.GetString(_data, 0, _data.Length); }
    }
}