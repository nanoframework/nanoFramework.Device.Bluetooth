//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is used to advertise a GATT service.
    /// </summary>
    public sealed class GattServiceProvider : IGattServiceProvider
    {
        private readonly GattLocalService _service;

        GattServiceProviderAdvertisementStatus _status = GattServiceProviderAdvertisementStatus.Created;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private byte[] _deviceName;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDiscoverable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isConnectable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _serviceData;


        internal static readonly BluetoothEventListener _bluetoothEventManager = new BluetoothEventListener();

        internal GattServiceProvider(Guid serviceUuid)
        {
            _service = new GattLocalService(serviceUuid);
            
           NativeInitService();
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
            
            if (NativeStartAdvertising())
            {
                _status = GattServiceProviderAdvertisementStatus.Started;
            }
        }

        /// <summary>
        ///  Stop advertising the GATT service.
        /// </summary>
        public void StopAdvertising()
        {
            NativeStopAdvertising();

            _status = GattServiceProviderAdvertisementStatus.Stopped;
        }

        /// <summary>
        ///  Creates a new GATT service with the specified serviceUuid
        /// </summary>
        /// <param name="serviceUuid">The service UUID.</param>
        /// <returns>A GattServiceProviderResult object.</returns>
        public static GattServiceProviderResult Create(Guid serviceUuid)
        {
            GattServiceProvider serviceProvider = new GattServiceProvider(serviceUuid);

            return new GattServiceProviderResult(serviceProvider, BluetoothError.Success);
        }

        /// <summary>
        /// Gets the advertisement status of this GATT service.
        /// </summary>
        /// <Returns>The advertisement service.</Returns>
        public GattServiceProviderAdvertisementStatus AdvertisementStatus { get => _status; }

        /// <summary>
        /// Gets the GATT service.
        /// </summary>
        /// <returns>The GATT service.</returns>
        public GattLocalService Service { get => _service; }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeInitService();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartAdvertising();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertising();
        #endregion
    }
}