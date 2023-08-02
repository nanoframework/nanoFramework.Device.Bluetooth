//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using nanoFramework.Device.Bluetooth.Advertisement;
using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is used to define the GATT service advertisement parameters.
    /// </summary>
    public class GattServiceProviderAdvertisingParameters
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isDiscoverable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _isConnectable = true;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _serviceData;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private BluetoothLEAdvertisement _advertisement;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private bool _customAdvertisement;


        /// <summary>
        ///  Creates a new <see cref="GattServiceProviderAdvertisingParameters"/> object.
        /// </summary>
        public GattServiceProviderAdvertisingParameters()
        {
            _advertisement = new();
            
            // Not a Custom advertisement, so set Flags & Local Name 
            CustomAdvertisement = false; 
            
            _isDiscoverable = true;
        }

        /// <summary>
        /// Gets or sets a boolean indicating that the GATT service is discoverable.
        /// </summary>
        public bool IsDiscoverable
        {
            get => _isDiscoverable;
            set
            {
                _isDiscoverable = value;
                if (_isDiscoverable)
                {
                    _advertisement.Flags |= BluetoothLEAdvertisementFlags.GeneralDiscoverableMode;
                }
                else
                {
                    _advertisement.Flags &= ~BluetoothLEAdvertisementFlags.GeneralDiscoverableMode;
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates if the GATT service is connect-able.
        /// </summary>
        public bool IsConnectable { get => _isConnectable; set => _isConnectable = value; }

        /// <summary>
        /// For Bluetooth Low Energy, this parameter adds an additional **ServiceData** section
        /// to the advertisement payload for the service's service UUID if space is available.
        /// If the service data is added to the advertisement, then the service UUID will
        /// also be included in the same section in the advertisement.
        /// </summary>
        public Buffer ServiceData { get => _serviceData; set => _serviceData = value; }

        /// <summary>
        /// Gets the underlying <see cref="BluetoothLEAdvertisement"/> object for the <see cref="GattServiceProviderAdvertisingParameters"/> to enable extra 
        /// advertisement parameters to be set.
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get => _advertisement; }

        /// <summary>
        /// If set the <see cref="Advertisement"/> will not be filled in with the default data sections only data sections from
        /// the <see cref="Advertisement"/> property will be used. Default is <see langword="false"/>.
        /// </summary>
        public bool CustomAdvertisement 
        { 
            get => _customAdvertisement; 
            set
            {
                _customAdvertisement = value;
                if (_customAdvertisement)
                {
                    // Remove default data sections
                    Advertisement.RemoveSectionsOfType(BluetoothLEAdvertisementDataSectionType.Flags);
                    Advertisement.RemoveSectionsOfType(BluetoothLEAdvertisementDataSectionType.CompleteLocalName);
                }
                else
                {
                    // Add default data sections
                    Advertisement.Flags = BluetoothLEAdvertisementFlags.ClassicNotSupported |
                                           BluetoothLEAdvertisementFlags.GeneralDiscoverableMode;

                    // Set default device name
                    Advertisement.LocalName = BluetoothLEServer.Instance.DeviceName;
                }
            }
        }
    }
}
