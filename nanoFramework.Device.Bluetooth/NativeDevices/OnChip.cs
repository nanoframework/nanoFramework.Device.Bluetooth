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

        /// <inheritdoc />
        public void ReadRespondWithValue(ushort eventId, byte[] data)
        {
            NativeReadRespondWithValue(eventId, data);
        }

        /// <inheritdoc />
        public void ReadRespondWithProtocolError(ushort eventId, byte otherError)
        {
            NativeReadRespondWithProtocolError(eventId, otherError);
        }
        
        /// <inheritdoc />
        public byte[] WriteGetData(ushort eventId)
        {
            return NativeWriteGetData(eventId);
        }

        /// <inheritdoc />
        public void WriteRespond(ushort eventId)
        {
            NativeWriteRespond(eventId);
        }

        /// <inheritdoc />
        public void WriteRespondWithProtocolError(ushort eventId, byte protocolError)
        {
            NativeWriteRespondWithProtocolError(eventId, protocolError);
        }

        #region Dispose


        /// <inheritdoc />
        public void Dispose()
        {

        }

        #endregion

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeInitService();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern bool NativeStartAdvertising();

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertising();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeNotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeReadRespondWithValue(ushort eventID, byte[] value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeReadRespondWithProtocolError(ushort eventID, byte protocolError);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern byte[] NativeWriteGetData(ushort eventID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeWriteRespond(ushort eventID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeWriteRespondWithProtocolError(ushort eventID, byte protocolError);
        
        #endregion
    }
}
