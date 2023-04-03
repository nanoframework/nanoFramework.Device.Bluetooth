//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using System.Diagnostics;
using System.Text;

namespace nanoFramework.Device.Bluetooth.Spp
{
    /// <summary>
    /// Implementation of Nordic Serial SPP profile.
    /// </summary>
    public class NordicSpp : IBluetoothSpp
    {
        // UUID for Nordic UART service
        // https://developer.nordicsemi.com/nRF_Connect_SDK/doc/latest/nrf/libraries/bluetooth_services/services/nus.html#id4
        private Guid ServiceUUID = new Guid("6E400001-B5A3-F393-E0A9-E50E24DCCA9E");
        private Guid RxCharacteristicUUID = new Guid("6E400002-B5A3-F393-E0A9-E50E24DCCA9E");
        private Guid TxCharacteristicUUID = new Guid("6E400003-B5A3-F393-E0A9-E50E24DCCA9E");

        private readonly GattServiceProvider _serviceProvider;
        private readonly GattLocalCharacteristic _txCharacteristic;
        private bool _isConnected = false;

        /// <summary>
        /// Return true id client connected
        /// </summary>
        public bool IsConnected { get => _isConnected; }

        /// <summary>
        /// Event handler for receiving data
        /// </summary>
        public event IBluetoothSpp.RxDataEventHandler ReceivedData;

        /// <summary>
        /// Event Handler for connection state change
        /// </summary>
        public event IBluetoothSpp.ConnectedEventHandler ConnectedEvent;

        /// <summary>
        /// Constructor for Nordic serial SPP profile
        /// </summary>
        public NordicSpp()
        {
             _ = BluetoothLEServer.Instance;
             
            GattServiceProviderResult gspr = GattServiceProvider.Create(ServiceUUID);
            if (gspr.Error != nanoFramework.Device.Bluetooth.BluetoothError.Success)
            {
                throw new ArgumentException("Unable to create service");
            }

            _serviceProvider = gspr.ServiceProvider;

            // Define RX characteristic
            GattLocalCharacteristicParameters rxParam = new GattLocalCharacteristicParameters()
            {
                UserDescription = "RX Characteristic",
                CharacteristicProperties = GattCharacteristicProperties.Write | GattCharacteristicProperties.WriteWithoutResponse
            };

            GattLocalCharacteristicResult rxCharRes = _serviceProvider.Service.CreateCharacteristic(RxCharacteristicUUID, rxParam);
            if (rxCharRes.Error != nanoFramework.Device.Bluetooth.BluetoothError.Success)
            {
                throw new ArgumentException("Unable to create RX Characteristic");
            }

            GattLocalCharacteristic rxCharacteristic = rxCharRes.Characteristic;
            rxCharacteristic.WriteRequested += RxCharacteristic_WriteRequested;


            // Define TX characteristic
            GattLocalCharacteristicParameters txParam = new GattLocalCharacteristicParameters()
            {
                UserDescription = "TX Characteristic",
                CharacteristicProperties = GattCharacteristicProperties.Notify
            };

            GattLocalCharacteristicResult txCharRes = _serviceProvider.Service.CreateCharacteristic(TxCharacteristicUUID, txParam);
            if (txCharRes.Error != nanoFramework.Device.Bluetooth.BluetoothError.Success)
            {
                throw new ArgumentException("Unable to create TX Characteristic");
            }

            _txCharacteristic = txCharRes.Characteristic;
            _txCharacteristic.SubscribedClientsChanged += _txCharacteristic_SubscribedClientsChanged;
        }

        private void _txCharacteristic_SubscribedClientsChanged(GattLocalCharacteristic sender, object args)
        {
            _isConnected = (sender.SubscribedClients.Length > 0);

            // Fire event when connection state changes
            ConnectedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Start device advertising
        /// </summary>
        /// <param name="deviceName">Device name for Advertising</param>
        /// <returns></returns>
        public bool Start(string deviceName)
        {
            BluetoothLEServer.Instance.DeviceName = deviceName;

            GattServiceProviderAdvertisingParameters advParameters = new GattServiceProviderAdvertisingParameters
            {
                IsDiscoverable = true,
                IsConnectable = true
            };

            _serviceProvider.StartAdvertising(advParameters);

            return true;
        }

        /// <summary>
        /// Stop Nordic SPP UART device
        /// Stop advertising.
        /// </summary>
        public void Stop()
        {
            _serviceProvider?.StopAdvertising();
        }

        /// <summary>
        /// Send data bytes to connected client
        /// </summary>
        /// <param name="data">byte array to send</param>
        /// <returns></returns>
        public bool SendBytes(byte[] data)
        {
            DataWriter dr = new DataWriter();
            dr.WriteBytes(data);
            GattClientNotificationResult[] results = _txCharacteristic.NotifyValue(dr.DetachBuffer());
            if (results.Length > 0 && results[0].ProtocolError == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Send data as string
        /// </summary>
        /// <param name="data">string to send</param>
        /// <returns></returns>
        public bool SendString(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return SendBytes(bytes);
        }

        /// <summary>
        /// Event handler for Received data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="WriteRequestEventArgs"></param>
        private void RxCharacteristic_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            GattWriteRequest request = WriteRequestEventArgs.GetRequest();

            byte[] data = new byte[request.Value.Length];

            DataReader rdr = DataReader.FromBuffer(request.Value);
            rdr.ReadBytes(data);

            ReceivedData?.Invoke(this, new SppReceivedDataEventArgs(data));

            if (request.Option == GattWriteOption.WriteWithResponse)
            {
                request.Respond();
            }
        }
    }
}
