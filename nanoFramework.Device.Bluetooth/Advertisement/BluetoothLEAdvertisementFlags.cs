//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Specifies flags used to match flags contained inside a Bluetooth LE advertisement
    /// payload.
    /// </summary>
    [Flags]
    public enum BluetoothLEAdvertisementFlags : uint
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Bluetooth LE Limited Discoverable Mode.
        /// </summary>
        LimitedDiscoverableMode = 1,

        /// <summary>
        /// Bluetooth LE General Discoverable Mode.
        /// </summary>
        GeneralDiscoverableMode = 2,

        /// <summary>
        /// Bluetooth BR/EDR not supported.
        /// </summary>
        ClassicNotSupported = 4,

        /// <summary>
        /// Simultaneous Bluetooth LE and BR/EDR to same device capable (controller).
        /// </summary>
        DualModeControllerCapable = 8,

        /// <summary>
        /// Simultaneous Bluetooth LE and BR/EDR to same device capable (host)
        /// </summary>
        DualModeHostCapable = 16
    }
}