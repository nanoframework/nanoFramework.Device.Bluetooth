//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Runtime.Native;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

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
        private BluetoothLEAdvertisementPublisher _publisher;

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
        /// Add default Device Information Service.
        /// </summary>
        private void AddDeviceInformationService()
        {
            // Add and initialized Device Information defaults
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
            if (BluetoothNanoDevice.RunMode != BluetoothNanoDevice.Mode.Server)
            {
                BluetoothLEServer.Instance.Start();
            }

            // Make sure pairing attributes set
            BluetoothLEServer.Instance.Pairing.SetPairAttributes();

            // Save IsConnectable & IsDiscoverable options in Advertisement
            parameters.Advertisement.IsConnectable = parameters.IsConnectable;
            parameters.Advertisement.IsDiscovable = parameters.IsDiscoverable;

            // Initialize Service configuration data in native code.
            if (NativeInitializeServiceConfig(BluetoothLEServer._services))
            {
                // Add default data sections for Service Provider to Advertisement if
                // not custom.
                if (!parameters.CustomAdvertisement)
                {
                    BluetoothLEAdvertisementDataSectionType suType;
                    DataWriter suDataWriter = new();

                    // Add Service UUID
                    switch (Utilities.TypeOfUuid(_service.Uuid))
                    {
                        case Utilities.UuidType.Uuid16:
                            suType = BluetoothLEAdvertisementDataSectionType.CompleteList16uuid;
                            suDataWriter.WriteUInt16(Utilities.ConvertUuidToShortId(_service.Uuid));
                            break;

                        case Utilities.UuidType.Uuid32:
                            suType = BluetoothLEAdvertisementDataSectionType.CompleteList32uuid;
                            suDataWriter.WriteUInt32(Utilities.ConvertUuidToIntId(_service.Uuid));
                            break;

                        case Utilities.UuidType.Uuid128:
                        default:
                            suType = BluetoothLEAdvertisementDataSectionType.CompleteList128uuid;
                            suDataWriter.WriteUuid(_service.Uuid);
                            break;
                    }

                    // Add Service UUID with correct type for UUID size.
                    parameters.Advertisement.AddOrUpdateDataSection(
                        suType,
                        suDataWriter.DetachBuffer());

                    // Add Service Data including Service UUID if required.
                    if (parameters.ServiceData != null)
                    {
                        BluetoothLEAdvertisementDataSectionType sdType;
                        DataWriter sdDataWriter = new();

                        switch (Utilities.TypeOfUuid(_service.Uuid))
                        {
                            case Utilities.UuidType.Uuid16:
                                sdType = BluetoothLEAdvertisementDataSectionType.ServiceData16bitUuid;
                                sdDataWriter.WriteUInt16(Utilities.ConvertUuidToShortId(_service.Uuid));
                                break;

                            case Utilities.UuidType.Uuid32:
                                sdType = BluetoothLEAdvertisementDataSectionType.ServiceData32bitUuid;
                                sdDataWriter.WriteUInt32(Utilities.ConvertUuidToIntId(_service.Uuid));
                                break;

                            case Utilities.UuidType.Uuid128:
                            default:
                                sdType = BluetoothLEAdvertisementDataSectionType.ServiceData128bitUuid;
                                sdDataWriter.WriteUuid(_service.Uuid);
                                break;
                        }
                        // Write service data after UUID
                        sdDataWriter.WriteBytes(parameters.ServiceData.Data);

                        // Add Service Data with correct type for UUID size.
                        parameters.Advertisement.AddOrUpdateDataSection(
                            sdType,
                            sdDataWriter.DetachBuffer());
                    }
                }

                _publisher = new BluetoothLEAdvertisementPublisher(parameters.Advertisement);
                _publisher.StatusChanged += Publisher_StatusChanged;

                // Start advertising.
                _publisher.Start();
            }
        }

        private void Publisher_StatusChanged(object sender, BluetoothLEAdvertisementPublisherStatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case BluetoothLEAdvertisementPublisherStatus.Started:
                    _status = _publisher.DataNotFitInAdvertisement ?
                        GattServiceProviderAdvertisementStatus.StartedWithoutAllAdvertisementData :
                        GattServiceProviderAdvertisementStatus.Started;
                    break;

                case BluetoothLEAdvertisementPublisherStatus.Stopped:
                    _status = GattServiceProviderAdvertisementStatus.Stopped;

                    NativeDisposeServiceConfig();
                    BluetoothNanoDevice.RunMode = BluetoothNanoDevice.Mode.NotRunning;
                    break;

                case BluetoothLEAdvertisementPublisherStatus.Aborted:
                    _status = GattServiceProviderAdvertisementStatus.Aborted;
                    break;
            }
        }

        /// <summary>
        ///  Stops advertising the current GATT service.
        /// </summary>
        public void StopAdvertising()
        {
            // Stop advertising and dispose of native data when stopped event received.
            _publisher.Stop();
        }

        /// <summary>
        ///  Creates a new GATT service with the specified serviceUuid.
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
                    return;
                }
            }

            BluetoothLEServer._services.Add(sprov);
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeInitializeServiceConfig(ArrayList services);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisposeServiceConfig();

        #endregion
    }
}
