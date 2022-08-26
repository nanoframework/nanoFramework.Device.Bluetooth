//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// The result of the pairing action.
    /// </summary>
    public enum DevicePairingResultStatus
    {
        /// <summary>
        /// The device object is now paired.
        /// </summary>
        Paired = 0,

        /// <summary>
        /// The device object is not in a state where it can be paired.
        /// </summary>
        NotReadyToPair = 1,

        /// <summary>
        /// The device object is not currently paired.
        /// </summary>
        NotPaired = 2,

        /// <summary>
        /// The device object has already been paired.
        /// </summary>
        AlreadyPaired = 3,

        /// <summary>
        /// The device object rejected the connection.
        /// </summary>
        ConnectionRejected = 4,

        /// <summary>
        /// The device object indicated it cannot accept any more incoming connections.
        /// </summary>
        TooManyConnections = 5,

        /// <summary>
        ///  The device object indicated there was a hardware failure.
        /// </summary>
        HardwareFailure = 6,

        /// <summary>
        /// The authentication process timed out before it could complete.
        /// </summary>
        AuthenticationTimeout = 7,

        /// <summary>
        /// The authentication protocol is not supported, so the device is not paired.
        /// </summary>
        AuthenticationNotAllowed = 8,

        /// <summary>
        /// Authentication failed, so the device is not paired. Either the device object
        /// or the application rejected the authentication.
        /// </summary>
        AuthenticationFailure = 9,

        /// <summary>
        /// here are no network profiles for this device object to use.
        /// </summary>
        NoSupportedProfiles = 10,

        /// <summary>
        /// The minimum level of protection is not supported by the device object or the
        /// application.
        /// </summary>
        ProtectionLevelCouldNotBeMet = 11,

        /// <summary>
        /// Your application does not have the appropriate permissions level to pair the
        /// device object.
        /// </summary>
        AccessDenied = 12,

        /// <summary>
        /// The ceremony data was incorrect.
        /// </summary>
        InvalidCeremonyData = 13,

        /// <summary>
        /// The pairing action was cancelled before completion.
        /// </summary>
        PairingCanceled = 14,

        /// <summary>
        ///  The device object is already attempting to pair or unpair.
        /// </summary>
        OperationAlreadyInProgress = 15,

        /// <summary>
        ///  Either the event handler wasn't registered or a required DevicePairingKinds was
        ///  not supported.
        /// </summary>
        RequiredHandlerNotRegistered = 16,

        /// <summary>
        ///  The application handler rejected the pairing.
        /// </summary>
        RejectedByHandler = 17,

        /// <summary>
        ///  The remove device already has an association.
        /// </summary>
        RemoteDeviceHasAssociation = 18,

        /// <summary>
        ///  An unknown failure occurred.
        /// </summary>
        Failed = 19
    }
}