//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    internal interface IGattServiceProviderResult
    {
        BluetoothError Error { get; }
        GattServiceProvider ServiceProvider { get; }
    }
}