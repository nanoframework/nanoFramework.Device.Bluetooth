using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth
{
    internal class BluetoothEventProcessor
    {
        internal BluetoothEventProcessor(INativeDevice nativeDevice)
        {
            // register events
            nativeDevice.OnReadRequested += OnReadRequested;
            nativeDevice.OnWriteRequested += OnWriteRequested;
            nativeDevice.OnClientSubscribed += OnClientSubscribed;
            nativeDevice.OnClientUnsubscribed += OnClientUnsubscribed;
        }

        private void OnReadRequested(object sender, NativeReadRequestedEventArgs e)
        {
            e.LocalCharacteristic.OnReadRequested(e.DescriptorId, new GattReadRequestedEventArgs(e.Id, null));
        }

        private void OnWriteRequested(object sender, NativeWriteRequestedEventArgs e)
        {
            e.LocalCharacteristic.OnWriteRequested(e.DescriptorId, new GattWriteRequestedEventArgs(e.Id, null));
        }

        private void OnClientSubscribed(object sender, NativeSubscribedClientsChangedEventArgs e)
        {
            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(e.Id));
            GattSubscribedClient sc = new GattSubscribedClient(gs);
            e.LocalCharacteristic.OnSubscribedClientsChanged(true, sc);
        }

        private void OnClientUnsubscribed(object sender, NativeSubscribedClientsChangedEventArgs e)
        {
            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(e.Id));
            GattSubscribedClient sc = new GattSubscribedClient(gs);
            e.LocalCharacteristic.OnSubscribedClientsChanged(false, sc);
        }
    }
}
