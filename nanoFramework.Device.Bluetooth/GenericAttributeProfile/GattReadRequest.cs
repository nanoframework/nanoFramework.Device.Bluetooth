//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a Bluetooth GATT read request.
    /// </summary>
    public class GattReadRequest
    {
        private Buffer _readValue;
        private readonly ushort _eventID;
        private readonly INativeDevice _nativeDevice;

        internal GattReadRequest(ushort eventID, INativeDevice nativeDevice)
        {
            _eventID = eventID;
            _nativeDevice = nativeDevice;
        }

        /// <summary>
        /// Responds to a read request with a value.
        /// </summary>
        /// <param name="value"> The value to respond with.</param>
        public void RespondWithValue(Buffer value)
        {
            _readValue = value;

            byte[] data = new byte[_readValue.Length];
            Array.Copy(_readValue.Data, data, (int)_readValue.Length);

            _nativeDevice.ReadRespondWithValue(_eventID, data);
        }

        /// <summary>
        /// Responds to the read request with a protocol error.
        /// </summary>
        /// <param name="protocolError">The protocol error to send. A list of errors with the byte values can be found in GattProtocolError.</param>
        public void RespondWithProtocolError(byte protocolError)
        {
            _nativeDevice.ReadRespondWithProtocolError(_eventID, (byte)BluetoothError.OtherError);
        }

        /// <summary>
        ///  Gets the buffer length of the read request.
        /// </summary>
        public uint Length { get => _readValue.Length; }
    }
}
