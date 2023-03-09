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
    /// This class contains the arguments for the Pairing event.
    /// </summary>
    public class DevicePairingRequestedEventArgs
    {
        private readonly DevicePairing _pairing;
        private readonly ushort _connectionHandle;
        private readonly DevicePairingKinds _kind;
        private readonly uint _pin;

        internal DevicePairingRequestedEventArgs(DevicePairing pairing, ushort connectionHandle, DevicePairingKinds kind, uint pin)
        {
            _pairing = pairing;
            _connectionHandle = connectionHandle;
            _kind = kind;
            _pin = pin;
        }

        /// <summary>
        /// Gets the kind of pairing associated with this pairing event.
        /// </summary>
        public DevicePairingKinds PairingKind { get => _kind; }

        /// <summary>
        /// Gets the pin associated with a pairing request.
        /// </summary>
        public uint Pin { get => _pin; }

        /// <summary>
        /// Accepts a PairingRequested event and pairs the device with the application.
        /// </summary>
        public void Accept()
        {
            NativeAcceptYesNo(_connectionHandle, _kind, 1);
        }

        /// <summary>
        ///  Accepts a PairingRequested event and pairs the device with the application. 
        ///  Requires a passkey for pairing purposes.
        /// </summary>
        /// <param name="passkey">The pass key for pairing.</param>
        public void Accept(int passkey)
        {
            NativeAcceptPasskey(_connectionHandle, _kind,  passkey);
        }

        /// <summary>
        /// Accepts a PairingRequested event and pairs the device with the application when
        /// a user name and password is required for pairing purposes.
        /// </summary>
        /// <param name="password">The password credential.</param>
        public void AcceptWithPasswordCredential(PasswordCredential password)
        {
            NativeAcceptCredentials(_connectionHandle, _kind, Encoding.UTF8.GetBytes(password.UserName), Encoding.UTF8.GetBytes(password.Password));
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeAcceptYesNo(ushort connectionHandle, DevicePairingKinds kind, int YesNo);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeAcceptPasskey(ushort connectionHandle, DevicePairingKinds kind, int passkey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeAcceptCredentials(ushort connectionHandle, DevicePairingKinds kind, byte[] username, byte[] password);

        #endregion
    }
}