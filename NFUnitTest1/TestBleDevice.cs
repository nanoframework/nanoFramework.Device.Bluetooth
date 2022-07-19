//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Device.Bluetooth.NativeDevice;

namespace NfUnitTest1
{
    internal class TestBleDevice : INativeDevice
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<NativeReadRequestedEventArgs> OnReadRequested;
        public event EventHandler<NativeWriteRequestedEventArgs> OnWriteRequested;
        public event EventHandler<NativeSubscribedClientsChangedEventArgs> OnClientSubscribed;
        public event EventHandler<NativeSubscribedClientsChangedEventArgs> OnClientUnsubscribed;

        public void InitService()
        {
            throw new NotImplementedException();
        }

        public void AddCharacteristic(GattLocalCharacteristic characteristic)
        {
            throw new NotImplementedException();
        }

        public void RemoveCharacteristic(GattLocalCharacteristic characteristic)
        {
            throw new NotImplementedException();
        }

        public void StopAdvertising()
        {
            throw new NotImplementedException();
        }

        public void WriteRespondWithProtocolError(ushort eventId, Byte protocolError)
        {
            throw new NotImplementedException();
        }

        public void WriteRespond(ushort eventId)
        {
            throw new NotImplementedException();
        }

        public byte[] WriteGetData(ushort eventId)
        {
            throw new NotImplementedException();
        }

        public void ReadRespondWithProtocolError(ushort eventId, byte otherError)
        {
            throw new NotImplementedException();
        }

        public void ReadRespondWithValue(ushort eventId, byte[] data)
        {
            throw new NotImplementedException();
        }

        public int NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer)
        {
            throw new NotImplementedException();
        }

        public bool StartAdvertising(bool isConnectable, bool isDiscoverable, Buffer serviceData, byte[] deviceName, ArrayList services)
        {
            throw new NotImplementedException();
        }
    }
}

