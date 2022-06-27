namespace nanoFramework.Device.Bluetooth
{
    public interface INativeDevice
    {
        void InitService();
        bool StartAdvertising();
        void StopAdvertising();
        public int NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer);
    }
}
