//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;
using System.Threading;

using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Device.Bluetooth.Services;
using nanoFramework.Runtime.Native;

using nanoFramework.Device.Bluetooth.Spp;

/// <summary>
/// TestClient for use with BlueTooth test cases
/// 
/// Includes Standard Services:
/// - Environmental Sensor
/// - Device Information
///  
/// Plus Test Service for Settings values
/// </summary>
namespace TestClient
{
    public static class Program
    {
        static EnvironmentalSensorService EnvService;

        // Indexes to sensors
        static int iTempOut;
        static int iTempOutMax;
        static int iTempOutMin;
        static int iHumidity;

        public static void Main()
        {
            Console.WriteLine();
            Console.WriteLine("BlueTooth Test Client");

            // Define used custom Uuid 
            Guid serviceUuid = new("A7EEDF2C-DA87-4CB5-A9C5-5151C78B0066");
            Guid readStaticCharUuid = new("A7EEDF2C-DA89-4CB5-A9C5-5151C78B0057");

            Console.WriteLine("Create Primary service - UUID A7EEDF2C-DA87-4CB5-A9C5-5151C78B0066");

            BluetoothLEServer.Instance.DeviceName = "Test2";

            //The GattServiceProvider is used to create and advertise the primary service definition.
            //An extra device information service will be automatically created.
            GattServiceProviderResult result = GattServiceProvider.Create(serviceUuid);
            if (result.Error != BluetoothError.Success)
            {
                return;
            }

            GattServiceProvider serviceProvider = result.ServiceProvider;

            // Get created Primary service from provider
            GattLocalService service = serviceProvider.Service;

            #region Static read characteristic
            // Now we add an characteristic to service
            // If the read value is not going to change then you can just use a Static value
            DataWriter sw = new();
            sw.WriteString("This is a test client");

            GattLocalCharacteristicResult characteristicResult = service.CreateCharacteristic(readStaticCharUuid,
                 new GattLocalCharacteristicParameters()
                 {
                     CharacteristicProperties = GattCharacteristicProperties.Read,
                     UserDescription = "My Static Characteristic",
                     StaticValue = sw.DetachBuffer()
                 });

            if (characteristicResult.Error != BluetoothError.Success)
            {
                // An error occurred.
                return;
            }
            #endregion

            Console.WriteLine("Create Device Information Service");

            // === Device Information Service ===
            DeviceInformationServiceService DifService = new(
                    "nanoFramework",
                    "Test Client",
                    null, // no serial number
                    "v1.0",
                    SystemInfo.Version.ToString(),
                    "");

            Console.WriteLine("Create Environmental Sensor Service");

            // === Environmental Sensor Service ===
            // https://www.bluetooth.com/specifications/specs/environmental-sensing-service-1-0/
            // This service exposes measurement data from an environmental sensors.
            EnvService = new EnvironmentalSensorService();

            // Add sensors to service, return index so sensor can be updated later.
            iTempOut = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Outside Temp");
            iTempOutMax = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Max Outside Temp", EnvironmentalSensorService.Sampling.Maximum);
            iTempOutMin = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Temperature, "Min Outside Temp", EnvironmentalSensorService.Sampling.Minimum);
            iHumidity = EnvService.AddSensor(EnvironmentalSensorService.SensorType.Humidity, "OUtside Humidty");

            // Update initial sensor values 
            EnvService.UpdateValue(iTempOut, 23.4F);
            EnvService.UpdateValue(iTempOutMax, 28.1F);
            EnvService.UpdateValue(iTempOutMin, 7.5F);
            EnvService.UpdateValue(iHumidity, 63.3F);

            // TestService test = new TestService(serviceProvider);
            // test.CommandRX += Test_CommandRX;

            #region Start Advertising
            Console.WriteLine("Start Advertising");

            // Once all the Characteristics/Services have been created you need to advertise so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it with a specific device name.
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                IsConnectable = true,
                IsDiscoverable = true
            });
            #endregion

            Thread.Sleep(60000);

            while (true)
            {
                float t1 = 23.4F;
                float t3 = 7.5F;

                // Up
                while (t1 < 120)
                {
                    t1 += 1.3F;
                    t3 += 2.1F;

                    EnvService.UpdateValue(iTempOut, t1);
                    EnvService.UpdateValue(iTempOutMin, t3);
                    Thread.Sleep(5000);
                }

                // Up
                while (t1 > -50F)
                {
                    t1 -= 1.3F;
                    t3 -= 2.1F;

                    EnvService.UpdateValue(iTempOut, t1);
                    EnvService.UpdateValue(iTempOutMin, t3);
                    Thread.Sleep(5000);
                }

            }


            Thread.Sleep(Timeout.Infinite);
        }

        // Receive test commands
        private static void Test_CommandRX(TestService sender, string args)
        {
            
        }
    }
}
