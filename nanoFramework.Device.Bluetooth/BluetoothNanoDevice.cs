//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

//
//  Static class to hold general device parameters
//  and controls the current run mode.
//  As we can't run both Server or Central/Client at same time
//

using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth
{
    internal static class BluetoothNanoDevice
    {
        internal enum Mode { NotRunning, Server, Scanning, Central };

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static string _deviceName = "";

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static Mode _mode = Mode.NotRunning;
        
        static BluetoothNanoDevice()
        {
            NativeInitilise();
        }

        internal static string DeviceName { get => _deviceName; set => _deviceName = value; }

        /// <summary>
        /// Get/Set the current mode of the device.
        /// </summary>
        internal static Mode RunMode
        {
            get => _mode;
            set
            {
                _mode = value;
                NativeSetOperationMode(_mode, DeviceName);
            }
        }

        #region Native
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void NativeInitilise();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void NativeSetOperationMode(Mode mode, string deviceName);
        #endregion
    }
}
