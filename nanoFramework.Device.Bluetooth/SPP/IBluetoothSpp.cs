//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.Spp
{
    /// <summary>
    /// Interface for Bluetooth Serial Profile (SPP)
    /// </summary>
    public interface IBluetoothSpp
    {
        /// <summary>
        /// Receive data delegate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ReadDataEventArgs"></param>
        public delegate void RxDataEventHandler(IBluetoothSpp sender, SppReceivedDataEventArgs ReadDataEventArgs);

        /// <summary>
        /// Connected client status changed delegate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ConnectedEventHandler(IBluetoothSpp sender, EventArgs e);

        /// <summary>
        /// Received data event. Event is fired when data is received.
        /// </summary>
        public event RxDataEventHandler ReceivedData;

        /// <summary>
        /// Connected client status changed event, Fired when client connects or disconnects.
        /// Check isConnected property for current status.
        /// </summary>
        public event ConnectedEventHandler ConnectedEvent;

        /// <summary>
        /// Returns true is a client is connected.
        /// </summary>
        bool IsConnected{ get; }

        /// <summary>
        /// Start the SPP advertising.
        /// </summary>
        /// <param name="deviceName">Device name to use in advert.</param>
        /// <returns></returns>
        bool Start(string deviceName);

        /// <summary>
        /// Stop advertising and close down.
        /// </summary>
        void Stop();

        /// <summary>
        /// Send bytes to connected client.
        /// </summary>
        /// <param name="data">Byte[] to send.</param>
        /// <returns>True if send was successful.</returns>
        bool SendBytes(byte[] data);

        /// <summary>
        /// Send string to connected client.
        /// </summary>
        /// <param name="data">String data to send.</param>
        /// <returns>True if send was successful.</returns>
        bool SendString(string data);
    }
}
