//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Interface to be implemented by native devices.
    /// </summary>
    public interface INativeDevice : IDisposable
    {
        /// <summary>
        /// Initializes the Gatt service.
        /// </summary>
        void InitService();

        /// <summary>
        /// Starts Advertising the Gatt service.
        /// </summary>
        /// <returns>if advertising started correctly.</returns>
        bool StartAdvertising(bool isDiscoverable, bool isConnectable, byte[] deviceName, ArrayList services);

        /// <summary>
        /// Stops advertising the Gatt service.
        /// </summary>
        void StopAdvertising();

        /// <summary>
        /// Sends a notification to a Gatt subscribed client.
        /// </summary>
        /// <param name="connection">The device id.</param>
        /// <param name="characteristicId">The characteristic id.</param>
        /// <param name="notifyBuffer">The buffer with notification data.</param>
        /// <returns></returns>
        int NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer);

        /// <summary>
        /// Responds to a read request with a value.
        /// </summary>
        /// <param name="eventId">The requests eventId.</param>
        /// <param name="data">The data to read.</param>
        void ReadRespondWithValue(ushort eventId, byte[] data);

        /// <summary>
        /// Responds to a read request with a protocol error.
        /// </summary>
        /// <param name="eventId">The requests eventId.</param>
        /// <param name="otherError">The protocol error to send. A list of errors with the byte values can be found in GattProtocolError.</param>
        void ReadRespondWithProtocolError(ushort eventId, byte otherError);

        /// <summary>
        /// Gets the Get data from the write request.
        /// </summary>
        /// <param name="eventId">The requests eventId.</param>
        /// <returns></returns>
        byte[] WriteGetData(ushort eventId);

        /// <summary>
        /// Responds to the write request.
        /// </summary>
        /// <param name="eventId">The requests eventId.</param>
        void WriteRespond(ushort eventId);

        /// <summary>
        /// Responds to the write request with a protocol error.
        /// </summary>
        /// <param name="eventId">The requests eventId.</param>
        /// <param name="protocolError">The protocol error to send. A list of errors with the byte values can be found in GattProtocolError.</param>
        void WriteRespondWithProtocolError(ushort eventId, byte protocolError);
    }
}
