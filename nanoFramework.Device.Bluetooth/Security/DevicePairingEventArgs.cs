//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// This class contains the arguments for the Pairing Complete event.
    /// </summary>
    public class DevicePairingEventArgs
    {
        private readonly ushort _connectionHandle;
        private readonly DevicePairingResultStatus _status;

        internal DevicePairingEventArgs(ushort connectionHandle, DevicePairingResultStatus status)
        {
            _connectionHandle = connectionHandle;
            _status = status;   
        }

        /// <summary>
        /// Status code of Pairing operation.
        /// </summary>
        public DevicePairingResultStatus Status { get { return _status; } }
    }
}