//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.TestFramework;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using NfUnitTest1;

namespace NFUnitTest1
{
    [TestClass]
    public class TestClass
    {
        Guid ServiceUuid1 = new Guid("CA761232-ED42-11CE-BACD-00AA0057B224");

        [TestMethod]
        public void CreateProvider()
        {
            GattServiceProvider serviceProvider = null;

            GattServiceProviderResult result = GattServiceProvider.Create(ServiceUuid1, new TestBleDevice());
            Assert.False(result.Error == BluetoothError.Success);

            serviceProvider = result.ServiceProvider;
            Assert.Null(serviceProvider, "Service provider is null");

            Assert.True(serviceProvider.AdvertisementStatus == GattServiceProviderAdvertisementStatus.Stopped, "Advertisement status should be stopped");
        }
    }
}
