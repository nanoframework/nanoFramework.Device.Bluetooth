//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  Contains the result of GATT write operations like WriteValueWithResult.
    /// </summary>
    public class GattWriteResult
    {
        private readonly GattCommunicationStatus _status;
        private readonly byte _protocolError;

        internal GattWriteResult(GattCommunicationStatus status, byte protocolError)
        {
            _status = status;
            _protocolError = protocolError;
        }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _protocolError; }

        /// <summary>
        /// Gets the status of the write result.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }
    }
}
