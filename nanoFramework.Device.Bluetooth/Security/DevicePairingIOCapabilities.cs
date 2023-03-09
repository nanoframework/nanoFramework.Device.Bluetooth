//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Kinds of IO capabilties used for pairing.
    /// </summary>
    public enum DevicePairingIOCapabilities
    {
        /// <summary>
        /// Device has just a display.
        /// </summary>
        DisplayOnly = 0,

        /// <summary>
        /// Device can output numeric data and has a way to input a Yes or No (buttons).
        /// </summary>
        DisplayYesNo = 1,

        /// <summary>
        /// Device has keyboard.
        /// </summary>
        KeyboardOnly = 2,

        /// <summary>
        /// Device has no output or input. 
        /// All pairing uses Just Works.
        /// </summary>
        NoInputNoOutput = 3,

        /// <summary>
        /// Device has Keyboard and Display (input/Output).
        /// </summary>
        KeyboardDisplay = 4
    }
}