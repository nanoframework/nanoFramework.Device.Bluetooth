//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of an asynchronous read operation of a GATT Characteristic
    /// or Descriptor value.
    /// </summary>
    public class GattReadResult
    {
        private readonly GattCommunicationStatus _status;
        private readonly byte _protocolError;
        private readonly Buffer _buffer;

        internal GattReadResult(Buffer buffer, GattCommunicationStatus status, byte protocolError)
        {
            _buffer = buffer;
            _status = status;
            _protocolError = protocolError;
        }

        /// <summary>
        /// Gets the status of an operation.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }

        /// <summary>
        /// Gets the value read from the device.
        /// </summary>
        public Buffer Value { get => _buffer; }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _protocolError; }
    }
}
