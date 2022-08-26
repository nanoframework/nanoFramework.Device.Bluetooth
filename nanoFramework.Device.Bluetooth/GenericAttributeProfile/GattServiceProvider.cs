//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using nanoFramework.Runtime.Native;
using nanoFramework.Device.Bluetooth;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is used to advertise GATT services.
    /// </summary>
    public class GattServiceProvider
    {
        GattServiceProviderAdvertisementStatus _status = GattServiceProviderAdvertisementStatus.Created;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattLocalService _service;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDiscoverable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isConnectable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _serviceData;
       
        internal GattServiceProvider(Guid serviceUuid)
        {
            // Add local service
            _service = new(serviceUuid);

            // Update services array
            UpdateServices(this);

            // Add default Device Information service after first service added
            if (BluetoothLEServer._services.Count == 1)
            {
                AddDeviceInformationService();
            }
        }

        /// <summary>
        /// Add default Device Information Service
        /// </summary>
        private void AddDeviceInformationService()
        {
            // Add and initialised Device Information defaults
            GattServiceProvider ServiceProvider = new GattServiceProvider(GattServiceUuids.DeviceInformation);
            GattLocalService dinfService = ServiceProvider.Service;

            // ManufacturerNameString Characteristic (0x2A29)
            DataWriter manufacturerName = new();
            manufacturerName.WriteString("nanoFramework");

            dinfService.CreateCharacteristic(GattCharacteristicUuids.ManufacturerNameString,
                new GattLocalCharacteristicParameters()
                {
                    StaticValue = manufacturerName.DetachBuffer(),
                    CharacteristicProperties = GattCharacteristicProperties.Read
                });

            // ModelNumberString Characteristic (0x2A24)
            DataWriter modelNumber = new();
            modelNumber.WriteString(SystemInfo.Platform);

            dinfService.CreateCharacteristic(GattCharacteristicUuids.ModelNumberString,
                new GattLocalCharacteristicParameters()
                {
                    StaticValue = modelNumber.DetachBuffer(),
                    CharacteristicProperties = GattCharacteristicProperties.Read
                });
        }

        /// <summary>
        /// Start advertising the GATT service.
        /// </summary>
        public void StartAdvertising()
        {
            StartAdvertising(new GattServiceProviderAdvertisingParameters());
        }

        /// <summary>
        /// Start advertising the GATT service.
        /// </summary>
        /// <param name="parameters">The advertising parameters.</param>
        public void StartAdvertising(GattServiceProviderAdvertisingParameters parameters)
        {
            // Check and switch to server mode
            if (BluetoothNanoDevice.RunMode != BluetoothNanoDevice.Mode.Server )
            {
                BluetoothLEServer.Instance.Start();
            }

            // Save parameters
            _isConnectable = parameters.IsConnectable;
            _isDiscoverable = parameters.IsDiscoverable;
            _serviceData = parameters.ServiceData;

            // Make sure pairing attributes set
            BluetoothLEServer.Instance.Pairing.SetPairAttributes();

            // Start advertising.
            // Native code will use the data provided by this GattServiceProvider instance to
            // initialise the BLE advert, service and characteristic definitions.
            if (NativeStartAdvertising(BluetoothLEServer._services))
            {
                _status = GattServiceProviderAdvertisementStatus.Started;
            }
        }

        #region Security

        /// <summary>
        /// Evaulate all services and work out default security requirements.
        /// </summary>
        /// <returns>
        /// Minimum DevicePairingProtectionLevel for servcies/characteristics.
        /// </returns>
        private DevicePairingProtectionLevel EvaluateSecurityPairingRequirements()
        {
            DevicePairingProtectionLevel _minProtectionLevel = DevicePairingProtectionLevel.Default;

            bool secureConnection = false;
            bool authentication = false;

            foreach (GattLocalService srv in BluetoothLEServer._services)
            {
                foreach (GattLocalCharacteristic chr in srv.Characteristics)
                {
                    if (chr.ReadProtectionLevel == GattProtectionLevel.EncryptionRequired ||
                        chr.ReadProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired)
                    {
                        secureConnection = true;
                    }

                    if (chr.ReadProtectionLevel == GattProtectionLevel.AuthenticationRequired ||
                        chr.ReadProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.AuthenticationRequired ||
                        chr.WriteProtectionLevel == GattProtectionLevel.EncryptionAndAuthenticationRequired)
                    {
                        authentication = true;
                    }
                }
            }

            if (secureConnection)
            {
                _minProtectionLevel = DevicePairingProtectionLevel.Encryption;
            }

            if (authentication)
            {
                _minProtectionLevel = DevicePairingProtectionLevel.EncryptionAndAuthentication;
            }

            return _minProtectionLevel;
        }

        #endregion Security


        /// <summary>
        ///  Stop advertising the current GATT service.
        /// </summary>
        internal void StopAdvertising()
        {
            // Stop advertising and dispose of native data.
            NativeStopAdvertising();

            _status = GattServiceProviderAdvertisementStatus.Stopped;

            BluetoothNanoDevice.RunMode = BluetoothNanoDevice.Mode.NotRunning;
        }

        /// <summary>
        ///  Creates a new GATT service with the specified serviceUuid
        /// </summary>
        /// <param name="serviceUuid">The service UUID.</param>
        /// <returns>A GattServiceProviderResult object.</returns>
        public static GattServiceProviderResult Create(Guid serviceUuid)
        {
            GattServiceProvider serviceProvider = new(serviceUuid);

            return new GattServiceProviderResult(serviceProvider, BluetoothError.Success);
        }

        /// <summary>
        /// Gets the advertisement status of this GATT service.
        /// </summary>
        /// <Returns>The advertisement service.</Returns>
        public GattServiceProviderAdvertisementStatus AdvertisementStatus { get => _status; }

        /// <summary>
        /// Gets the GATT local service for this provider.
        /// </summary>
        /// <returns>The GattLocalService object.</returns>
        public GattLocalService Service { get => _service; }

        private void UpdateServices(GattServiceProvider sprov)
        {
            for (int index = 0; index < BluetoothLEServer._services.Count; index++)
            {
                // Service provider with same UUID then replace
                if (((GattServiceProvider)BluetoothLEServer._services[index]).Service.Uuid.Equals(sprov._service.Uuid))
                {
                    // Replace existing service on index
                    BluetoothLEServer._services[index] = sprov;
                }
            }

            BluetoothLEServer._services.Add(sprov);
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartAdvertising(ArrayList services);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertising();

        #endregion
    }
}