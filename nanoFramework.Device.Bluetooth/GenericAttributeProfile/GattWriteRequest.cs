//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a GATT write request.
    /// </summary>
    public class GattWriteRequest
    {
        private readonly GattWriteOption _option = GattWriteOption.WriteWithResponse;
        private readonly uint _offset = 0;
        private readonly Buffer _value = null;

        private readonly ushort _eventID;

        internal GattWriteRequest(ushort eventID)
        {
            _eventID = eventID;

            // Get a copy of data from Native for this event
            byte[] data = NativeWriteGetData(eventID);

            // and save it
            _value = new Buffer(data);
        }

        /// <summary>
        /// Responds to the write request.
        /// </summary>
        public void Respond()
        {
            NativeWriteRespond(_eventID);
        }

        /// <summary>
        ///  Responds with a protocol error.
        /// </summary>
        /// <param name="protocolError">Error byte</param>
        public void RespondWithProtocolError(byte protocolError)
        {
            NativeWriteRespondWithProtocolError(_eventID, protocolError);
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        public uint Offset { get => _offset; }

        /// <summary>
        /// Gets the write request option.
        /// </summary>
        public GattWriteOption Option { get => _option; }

        /// <summary>
        /// Gets the buffer value of the write request.
        /// </summary>
        public Buffer Value { get => _value; }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern byte[] NativeWriteGetData(ushort eventID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeWriteRespond(ushort eventID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeWriteRespondWithProtocolError(ushort eventID, byte protocolError);

        #endregion
    }
}
