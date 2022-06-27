using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.NativeDevices
{
    public class OnChip : INativeDevice
    {
        public void InitService()
        {
            NativeInitService();
        }

        public bool StartAdvertising()
        {
            return NativeStartAdvertising();
        }

        public void StopAdvertising()
        {
            NativeStopAdvertising();
        }

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
        private extern int NativeNotifyClient(ushort connection, ushort CharacteristicId, byte[] notifyBuffer);

        #endregion
    }
}
