//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.TestFramework;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace NFUnitTests
{
    [TestClass]
    public class TestServiceProvider
    {
        Guid ServiceUuid1 = new("CA761232-ED42-11CE-BACD-00AA0057B224");

        [TestMethod]
        public void CreateGattServiceProvider()
        {
            GattServiceProvider serviceProvider;

            GattServiceProviderResult result = GattServiceProvider.Create(ServiceUuid1);
            Assert.IsFalse(result.Error == BluetoothError.Success);

            serviceProvider = result.ServiceProvider;
            Assert.IsNull(serviceProvider, "Service provider is null");

            Assert.IsTrue(serviceProvider.AdvertisementStatus == GattServiceProviderAdvertisementStatus.Stopped, "Advertisement status should be stopped");
        }
    }
}
