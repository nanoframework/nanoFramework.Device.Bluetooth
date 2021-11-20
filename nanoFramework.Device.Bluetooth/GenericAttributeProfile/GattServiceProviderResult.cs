//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class is the result of the Create operation.
    /// </summary>
    public sealed class GattServiceProviderResult : IGattServiceProviderResult
    {
        private readonly GattServiceProvider _serviceProvider;
        private readonly BluetoothError _error;
        
        internal GattServiceProviderResult(GattServiceProvider serviceProvider, BluetoothError error)
        {
            _serviceProvider = serviceProvider;
            _error = error;
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        public BluetoothError Error { get => _error; }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public GattServiceProvider ServiceProvider { get => _serviceProvider; }
    }
}