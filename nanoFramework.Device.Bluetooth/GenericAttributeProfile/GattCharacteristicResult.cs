//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Contains the result of GetCharacteristicsForUuid and GetCharacteristics
    /// </summary>
    public class GattCharacteristicsResult
    {
        private readonly byte _protocolError;
        private readonly ArrayList _characteristics;
        private readonly GattCommunicationStatus _status;

        internal GattCharacteristicsResult(byte ProtocolError, ArrayList Characteristics, GattCommunicationStatus Status)
        {
            _protocolError = ProtocolError;
            _characteristics = Characteristics;
            _status = Status;
        }

        /// <summary>
        /// Gets the characteristics.
        /// returning an Array of GattCharacteristic objects.
        /// </summary>
        public GattCharacteristic[] Characteristics { get => (GattCharacteristic[])_characteristics.ToArray(typeof(GattCharacteristic)); }

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
