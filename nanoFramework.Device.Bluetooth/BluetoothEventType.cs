//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Runtime.Events;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Event type for Bluetooth events coming from Native
    /// </summary>
    public enum BluetoothEventType
    {
        /// <summary>
        /// Attribute Read
        /// </summary>
        Read,

        /// <summary>
        /// Attribute write
        /// </summary>
        Write,

        /// <summary>
        /// Client Subscribed
        /// </summary>
        ClientSubscribed,

        /// <summary>
        /// Client unsubscribed or connection terminated
        /// </summary>
        ClientUnsubscribed,

        // ==== BLE Scanning Events ====

        /// <summary>
        /// Advertisement discovered when scanning
        /// </summary>
        AdvertisementDiscovered,

        /// <summary>
        /// Discovery Scan complete
        /// </summary>
        ScanningComplete,

        // ==== BLE Central/Client events ====

        /// <summary>
        /// Fires when native connect to device completes
        /// </summary>
        ConnectComplete,

        /// <summary>
        /// The connection has disconnected.
        /// </summary>
        ConnectionDisconnected,

        /// <summary>
        /// A Service discovered for a connection
        /// </summary>
        ServiceDiscovered,

        /// <summary>
        /// Service discovery has completed
        /// </summary>
        ServiceDiscoveryComplete,

        /// <summary>
        /// A characteristic discovered on Service
        /// </summary>
        CharacteristicDiscovered,

        /// <summary>
        /// The characteristic discovery has completed / error
        /// </summary>
        CharacteristicDiscoveryComplete,

        /// <summary>
        /// A descriptor discovered on Characteristic
        /// </summary>
        DescriptorDiscovered,

        /// <summary>
        /// The Descriptor discovery has completed / error
        /// </summary>
        DescriptorDiscoveryComplete,

        /// <summary>
        /// Characteristic read value complete, status=error
        /// </summary>
        AttributeReadValueComplete,

        /// <summary>
        /// Attribute Write value completed, status = error
        /// </summary>
        AttributeWriteValueComplete,

        /// <summary>
        /// Fired when a value on connected device has changed, notify.
        /// </summary>
        AttributeValueChanged

    }
}