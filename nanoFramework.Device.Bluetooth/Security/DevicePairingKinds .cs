//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Kinds of paring.
    /// </summary>
    public enum DevicePairingKinds
    {
        /// <summary>
        /// No pairing is supported.
        /// </summary>
        None,

        /// <summary>
        /// The application must confirm they wish to perform the pairing action. You can present an optional confirmation dialog to the user. 
        /// With a value of ConfirmOnly, call Accept from the event args of the PairingRequested event handler if you want the pairing to complete.
        /// </summary>
        ConfirmOnly = 1,

        /// <summary>
        /// The application must display the given PIN to the user. The user will then need to enter or confirm that PIN on the device that is being paired. 
        /// With a value of DisplayPin, call Accept from the event args of the PairingRequested event handler if you want the pairing to complete. 
        /// If your application cancels the pairing at this point, the device might still be paired. This is because the system and the target 
        /// device don't need any confirmation for this DevicePairingKinds value.
        /// </summary>
        DisplayPin = 2,

        /// <summary>
        /// The application must request a PIN from the user. The PIN will typically be displayed on the target device. With a value of ProvidePin, 
        /// call Accept from the event args of the PairingRequested event handler if you want the pairing to complete. Pass in the PIN as a parameter.
        /// </summary>
        ProvidePin = 4,

        /// <summary>
        /// The application must display the given PIN to the user and ask the user to confirm that the PIN matches the one show on the target device. 
        /// With a value of ConfirmPinMatch, call Accept from the event args of the PairingRequested event handler if you want the pairing to complete.
        /// </summary>
        ConfirmPinMatch = 8,
    }
}
