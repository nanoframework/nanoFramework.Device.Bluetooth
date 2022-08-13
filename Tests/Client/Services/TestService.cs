// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth.Services
{
    /// <summary>
    /// Allows commands to be sent to test device
    /// </summary>
    public class TestService
    {
        private readonly GattLocalService _testService;

        public Guid serviceUUID = new Guid("5CB16844-E11B-43C2-A2DD-45685828488F");
        public Guid rxCommandUUID = new Guid("CC168A62-EA8C-489D-9AF1-57F2F75A713F");

        public delegate void CommandRXHandler(TestService sender, string args);
        public event CommandRXHandler CommandRX;

        /// <summary>
        /// Create a test service
        /// </summary>
        /// <param name="provider"></param>
        public TestService(GattServiceProvider provider)
        {
            // Add new test  Service to provider
            _testService = provider.AddService(serviceUUID);

            GattLocalCharacteristicParameters rxCommandPar = new GattLocalCharacteristicParameters()
            {
                UserDescription = "RX command",
                CharacteristicProperties = GattCharacteristicProperties.Write | GattCharacteristicProperties.WriteWithoutResponse
            };

            GattLocalCharacteristicResult rxCommandRes = provider.Service.CreateCharacteristic(rxCommandUUID, rxCommandPar);
            if (rxCommandRes.Error != nanoFramework.Device.Bluetooth.BluetoothError.Success)
            {
                throw new ArgumentException("Unable to create RX Command");
            }

            GattLocalCharacteristic rxCommand = rxCommandRes.Characteristic;
            rxCommand.WriteRequested += RxCommand_WriteRequested;

        }


        private void RxCommand_WriteRequested(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs)
        {
            DataReader rdr = DataReader.FromBuffer(WriteRequestEventArgs.GetRequest().Value);

            byte[] bytes = new byte[rdr.UnconsumedBufferLength];
            rdr.ReadBytes(bytes);
            string command = Encoding.UTF8.GetString(bytes,0, bytes.Length);

            CommandRX?.Invoke(this, command);
        }
    }
}