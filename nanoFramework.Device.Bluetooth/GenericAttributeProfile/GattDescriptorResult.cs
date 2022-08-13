//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// The result of descriptor operations like GattCharacteristic.GetDescriptors
    /// </summary>
    public class GattDescriptorsResult
    {
        private readonly byte _protocolError;
        private readonly ArrayList _descriptors;
        private readonly GattCommunicationStatus _status;

        internal GattDescriptorsResult(byte ProtocolError, ArrayList Descriptors, GattCommunicationStatus Status)
        {
            _protocolError = ProtocolError;
            _descriptors = Descriptors;
            _status = Status;
        }

        /// <summary>
        /// Gets the descriptors.
        /// returning an Array of GattDescriptor objects.
        /// </summary>
        public GattDescriptor[] Descriptors { get => (GattDescriptor[])_descriptors.ToArray(typeof(GattDescriptor)); }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _protocolError; }

        /// <summary>
        /// Gets the communication status of the operation.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }
    }
}