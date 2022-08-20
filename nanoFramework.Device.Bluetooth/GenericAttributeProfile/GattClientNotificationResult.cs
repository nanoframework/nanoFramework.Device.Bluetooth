//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// The result of a NotifyValue
    /// </summary>
    public class GattClientNotificationResult
    {
        private readonly byte _result;
        private readonly GattCommunicationStatus _status;
        private readonly GattSubscribedClient _client;
        private readonly ushort _bytesSent;

        internal GattClientNotificationResult(byte result, GattCommunicationStatus status, GattSubscribedClient client, ushort bytesSent)
        {
            _result = result;
            _status = status;
            _client = client;
            _bytesSent = bytesSent;
        }

        /// <summary>
        /// Gets the protocol error.
        /// </summary>
        public byte ProtocolError { get => _result; }

        /// <summary>
        /// Gets the GATT communication status.
        /// </summary>
        public GattCommunicationStatus Status { get => _status; }

        /// <summary>
        /// Gets the subscribed client.
        /// </summary>
        public GattSubscribedClient SubscribedClient { get => _client; }

        /// <summary>
        /// Gets the bytes that were sent.
        /// </summary>
        public ushort BytesSent { get => _bytesSent; }

    }
}
