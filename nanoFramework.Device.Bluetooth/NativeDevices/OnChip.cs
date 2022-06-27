using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.NativeDevices
{
    /// <summary>
    /// The implementation for devices that use the native call system.
    /// </summary>
    public class OnChip : INativeDevice
    {
        /// <inheritdoc />
        public void InitService()
        {
            NativeInitService();
        }

        /// <inheritdoc />
        public bool StartAdvertising()
        {
            return NativeStartAdvertising();
        }

        /// <inheritdoc />
        public void StopAdvertising()
        {
            NativeStopAdvertising();
        }

        /// <inheritdoc />
        public int NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer)
        {
            return NativeNotifyClient(connection, characteristicId, notifyBuffer);
        }
        
        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeInitService();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartAdvertising();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertising();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeNotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer);

        #endregion
    }
}
