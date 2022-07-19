//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.NativeDevice
{
    /// <summary>
    /// Interface to be implemented by native devices.
    /// </summary>
    public interface INativeDevice : IDisposable
    {
        /// <summary>
        /// Event triggered when A read from the device is requested over bluetooth.
        /// </summary>
        event EventHandler<NativeReadRequestedEventArgs> OnReadRequested;
        
        /// <summary>
        /// Event triggered when a write to the device is requested over bluetooth.
        /// </summary>
        event EventHandler<NativeWriteRequestedEventArgs> OnWriteRequested;
        
        /// <summary>
        /// Event triggered when a client has subscribed to a characteristic.
        /// </summary>
        event EventHandler<NativeSubscribedClientsChangedEventArgs> OnClientSubscribed;
        
        /// <summary>
        /// Event triggered when a client has unsubscribed from a characteristic.
        /// </summary>
        event EventHandler<NativeSubscribedClientsChangedEventArgs> OnClientUnsubscribed;

        /// <summary>
        /// Initializes the Gatt service.
        /// </summary>
        void InitService();

        /// <summary>
        /// Adds a characteristic to the service.
        /// </summary>
        /// <param name="characteristic">the characteristic to add</param>
        void AddCharacteristic(GattLocalCharacteristic characteristic);

        /// <summary>
        /// Removes a characteristic from the service.
        /// </summary>
        /// <param name="characteristic">the characteristic to remove</param>
        void RemoveCharacteristic(GattLocalCharacteristic characteristic);

        /// <summary>
        /// Starts Advertising the Gatt service.
        /// </summary>
        /// <param name="isConnectable">if the Gatt service should be connectable</param>
        /// <param name="isDiscoverable">if the Gatt service should be discoverable</param>
        /// <param name="serviceData">adds an additional **ServiceData** section
        /// to the advertisement payload for the service's service UUID.</param>
        /// <param name="deviceName">the name of the BLE device when advertising</param>
        /// <param name="services">List of GattLocalServices.</param>
        /// <returns>if advertising started correctly.</returns>
        bool StartAdvertising(bool isConnectable, bool isDiscoverable, Buffer serviceData, byte[] deviceName, ArrayList services);

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
        /// Reads the data written to the device after a write request.
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
