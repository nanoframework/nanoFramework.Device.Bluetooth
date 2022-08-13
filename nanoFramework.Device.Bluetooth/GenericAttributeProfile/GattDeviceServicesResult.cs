//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// The status of GetIncludedServicesForUuid and GetIncludedServicesForUuid
    /// </summary>
    public class GattDeviceServicesResult 
    {
        private readonly byte _protocolError;
        private readonly ArrayList _services;
        private readonly GattCommunicationStatus _status;

        internal GattDeviceServicesResult(byte ProtocolError, ArrayList Services, GattCommunicationStatus Status)
        {
            _protocolError = ProtocolError;
            _services = Services;
            _status = Status;
        }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _protocolError; }

        /// <summary>
        /// Gets the services.
        /// returning an Array of GattDeviceService objects.
        /// </summary>
        public GattDeviceService[] Services { get => (GattDeviceService[])_services.ToArray(typeof(GattDeviceService)); }

        /// <summary>
        /// Gets the communication status of the operation.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }
    }
}