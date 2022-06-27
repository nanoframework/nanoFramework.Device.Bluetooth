//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using System.Collections;
using System.Runtime.CompilerServices;
using nanoFramework.Runtime.Native;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is used to advertise GATT services.
    /// </summary>
    public sealed class GattServiceProvider
    {
        private readonly ArrayList _services;
        private readonly INativeDevice _nativeDevice;

        GattServiceProviderAdvertisementStatus _status = GattServiceProviderAdvertisementStatus.Created;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private byte[] _deviceName;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDiscoverable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isConnectable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _serviceData;


        internal static BluetoothEventListener _bluetoothEventManager ;

        internal GattServiceProvider(Guid serviceUuid, INativeDevice nativeDevice)
        {
            _services = new ArrayList();
            _nativeDevice = nativeDevice;
            _bluetoothEventManager = new BluetoothEventListener(_nativeDevice);

            // Add primary
            AddService(serviceUuid);

            // Add default Device Information service 
            AddDeviceInformationService();

            _nativeDevice.InitService();
        }

        /// <summary>
        /// Add default Device Information Service
        /// </summary>
        private void AddDeviceInformationService()
        {
            // Add and initialised Device Information defaults
            GattLocalService dinfService = AddService(GattServiceUuids.DeviceInformation);

            // ManufacturerNameString Characteristic (0x2A29)
            DataWriter manufacturerName = new DataWriter();
            manufacturerName.WriteString("nanoFramework");

            dinfService.CreateCharacteristic(GattCharacteristicUuids.ManufacturerNameString,
                new GattLocalCharacteristicParameters()
                {
                    StaticValue = manufacturerName.DetachBuffer(),
                    CharacteristicProperties = GattCharacteristicProperties.Read
                });

            // ModelNumberString Characteristic (0x2A24)
            DataWriter modelNumber = new DataWriter();
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
            // Save parameters
            _isConnectable = parameters.IsConnectable;
            _isDiscoverable = parameters.IsDiscoverable;
            _serviceData = parameters.ServiceData;

            _deviceName = Encoding.UTF8.GetBytes(parameters.DeviceName);

            if (_nativeDevice.StartAdvertising())
            {
                _status = GattServiceProviderAdvertisementStatus.Started;
            }
        }

        /// <summary>
        ///  Stop advertising the GATT service.
        /// </summary>
        public void StopAdvertising()
        {
            _nativeDevice.StopAdvertising();

            _status = GattServiceProviderAdvertisementStatus.Stopped;
        }

        /// <summary>
        ///  Creates a new GATT service with the specified serviceUuid using the specified nativeDevice
        /// </summary>
        /// <param name="serviceUuid">The service UUID.</param>
        /// <param name="nativeDevice">The native device.</param>
        /// <returns>A GattServiceProviderResult object.</returns>
        public static GattServiceProviderResult Create(Guid serviceUuid, INativeDevice nativeDevice)
        {
            GattServiceProvider serviceProvider = new GattServiceProvider(serviceUuid, nativeDevice);

            return new GattServiceProviderResult(serviceProvider, BluetoothError.Success);
        }

        /// <summary>
        /// Gets the advertisement status of this GATT service.
        /// </summary>
        /// <Returns>The advertisement service.</Returns>
        public GattServiceProviderAdvertisementStatus AdvertisementStatus { get => _status; }

        /// <summary>
        /// Gets the GATT primary service.
        /// </summary>
        /// <returns>The primary service.</returns>
        public GattLocalService Service { get => Services[0]; }

        /// <summary>
        /// Get an array of all associated services for this service provider.
        /// </summary>
        /// <remarks>
        /// The primary service will be index 0 followed by the Device Information at index 1.
        /// Any other Services added to provider will follow these in the order they were created.
        /// </remarks>
        public GattLocalService[] Services { get { return (GattLocalService[])_services.ToArray(typeof(GattLocalService)); } }

        /// <summary>
        /// Creates a new service or replaces an existing service.
        /// Created service is added to this service provider.
        /// </summary>
        /// <param name="serviceUuid">Uuid for the service to be created/replaced.</param>
        /// <returns>
        /// Returns the created service.
        /// </returns>
        public GattLocalService AddService(Guid serviceUuid)
        {
            for (int index = 0; index < _services.Count; index++)
            {
                if (((GattLocalService)_services[index]).Uuid.Equals(serviceUuid))
                {
                    // Replace existing service on index
                    GattLocalService replacementService = new GattLocalService(serviceUuid, _nativeDevice);
                    _services[index] = replacementService;
                    return replacementService;
                }
            }

            // Add new service
            GattLocalService newService = new GattLocalService(serviceUuid, _nativeDevice);
            _services.Add(newService);
            return newService;
        }

    }
}