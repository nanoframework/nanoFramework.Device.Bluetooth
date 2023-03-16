//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

//
//  Static class to hold general device parameters
//  and controls the current run mode.
//  As we can't run both Server or Central/Client at same time
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth
{
    internal static class BluetoothNanoDevice
    {
        internal enum Mode { NotRunning, Server, Client };

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static string _deviceName = "nanoFramework";

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static ushort _appearance = 0;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private static Mode _mode = Mode.NotRunning;
        
        static BluetoothNanoDevice()
        {
            NativeInitilise();
        }

        internal static string DeviceName { get => _deviceName; set => _deviceName = value; }

        internal static ushort Appearance { get => _appearance; set => _appearance = value; }

        /// <summary>
        /// Checks if current mode enabled otherwise switch to mode.
        /// </summary>
        /// <param name="expectedMode">The expected mode.</param>
        /// <exception cref="InvalidOperationException">When mode is unexpected. i.e switching directly from client to server modes.</exception>
        internal static void CheckMode(Mode expectedMode)
        {
            if (RunMode != expectedMode)
            {
                // Set new run mode. 
                BluetoothNanoDevice.RunMode = expectedMode;
            }
        }

        /// <summary>
        /// Get/Set the current mode of the device.
        /// </summary>
        /// <exception cref="InvalidOperationException">When mode is unexpected from NativeSetOperationMode.</exception>
        internal static Mode RunMode
        {
            get => _mode;
            set
            {
                _mode = value;
                NativeSetOperationMode(_mode, DeviceName, Appearance);
            }
        }

        #region Native
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void NativeInitilise();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void NativeSetOperationMode(Mode mode, string deviceName, ushort appearance);
        #endregion
    }
}
