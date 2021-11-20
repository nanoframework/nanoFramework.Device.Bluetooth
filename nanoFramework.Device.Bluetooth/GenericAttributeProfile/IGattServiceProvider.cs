//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    internal interface IGattServiceProvider
    {
        void StartAdvertising();
        void StartAdvertising(GattServiceProviderAdvertisingParameters parameters);
        void StopAdvertising();

        GattServiceProviderAdvertisementStatus AdvertisementStatus { get; }
        GattLocalService Service { get; }
    }
}