//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    internal interface IGattServiceProviderAdvertisingParameters
    {
        bool IsConnectable { get; set; }
        bool IsDiscoverable { get; set; }
    }
}