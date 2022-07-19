//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a GATT write request.
    /// </summary>
    public sealed class GattWriteRequest
    {
        private readonly GattWriteOption _option = GattWriteOption.WriteWithResponse;
        private readonly uint _offset = 0;
        private readonly Buffer _value;

        private readonly ushort _eventId;

        internal GattWriteRequest(ushort eventId)
        {
            _eventId = eventId;

            // Get a copy of data from Native for this event
            byte[] data = GattServiceProvider.NativeDevice.WriteGetData(eventId);

            // and save it
            _value = new Buffer(data);
        }

        /// <summary>
        /// Responds to the write request.
        /// </summary>
        public void Respond()
        {
            GattServiceProvider.NativeDevice.WriteRespond(_eventId);
        }

        /// <summary>
        ///  Responds with a protocol error.
        /// </summary>
        /// <param name="protocolError">Error byte</param>
        public void RespondWithProtocolError(byte protocolError)
        {
            GattServiceProvider.NativeDevice.WriteRespondWithProtocolError(_eventId, protocolError);
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
    }
}
