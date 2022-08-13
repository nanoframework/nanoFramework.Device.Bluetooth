//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the result of reading a GATT Client CharacteristicConfigurationClientDescriptor value.
    /// </summary>
    public class GattReadClientCharacteristicConfigurationDescriptorResult
    {
        private readonly GattClientCharacteristicConfigurationDescriptorValue _config;
        private readonly byte _protocolError;
        private readonly GattCommunicationStatus _status;

        internal GattReadClientCharacteristicConfigurationDescriptorResult(
            GattClientCharacteristicConfigurationDescriptorValue value,
            GattCommunicationStatus status,
            byte protocolError)
        {
            _config = value;
            _status = status;
            _protocolError = protocolError;
        }

        /// <summary>
        /// Gets the result of an read operation.
        /// </summary>
        public GattClientCharacteristicConfigurationDescriptorValue ClientCharacteristicConfigurationDescriptor { get => _config; }

        /// <summary>
        /// Gets the status of an operation.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _protocolError; }
    }
}
