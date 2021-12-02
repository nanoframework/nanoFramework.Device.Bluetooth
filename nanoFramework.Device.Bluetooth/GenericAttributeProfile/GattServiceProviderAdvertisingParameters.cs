//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is used to define the GATT service advertisement parameters.
    /// </summary>
    public sealed class GattServiceProviderAdvertisingParameters
    {
        const string _defaultDeviceName = "nanoFramework";

        private string _deviceName = _defaultDeviceName;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDiscoverable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isConnectable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _serviceData;

        /// <summary>
        ///  Creates a new GattServiceProviderAdvertisingParameters object.
        /// </summary>
        public GattServiceProviderAdvertisingParameters()
        {
        }

        /// <summary>
        /// Gets or sets a boolean indicating that the GATT service is discoverable.
        /// </summary>
        public bool IsDiscoverable { get => _isDiscoverable; set => _isDiscoverable = value; }

        /// <summary>
        /// Gets or sets a boolean that indicates if the GATT service is connect-able.
        /// </summary>
        public bool IsConnectable { get => _isConnectable; set => _isConnectable = value; }

        /// <summary>
        /// Friendly device name used for advertising service.
        /// Default "nanoFramework"
        /// </summary>
        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _deviceName = _defaultDeviceName;
                }
                else
                {
                    _deviceName = value;
                }
            }
        }

        /// <summary>
        /// For Bluetooth Low Energy, this parameter adds an additional **ServiceData** section
        /// to the advertisement payload for the service's service UUID if space is available.
        /// If the service data is added to the advertisement, then the service UUID will
        /// also be included in the same section in the advertisement.
        /// </summary>
        public Buffer ServiceData { get => _serviceData; set => _serviceData = value; }
    }
}