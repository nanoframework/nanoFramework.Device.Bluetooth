namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// 
    /// </summary>
    public interface INativeDevice
    {
        /// <summary>
        /// Initializes the Gatt service.
        /// </summary>
        void InitService();
        
        /// <summary>
        /// Starts Advertising the Gatt service
        /// </summary>
        /// <returns>if advertising started correctly.</returns>
        bool StartAdvertising();
        
        /// <summary>
        /// Stops advertising the Gatt service
        /// </summary>
        void StopAdvertising();
        
        /// <summary>
        /// Sends a notification to a Gatt subscribed client.
        /// </summary>
        /// <param name="connection">The device id.</param>
        /// <param name="characteristicId">The characteristic id.</param>
        /// <param name="notifyBuffer">The buffer with notification data.</param>
        /// <returns></returns>
        public int NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer);
    }
}
