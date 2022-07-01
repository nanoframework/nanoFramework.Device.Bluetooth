//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using nanoFramework.Device.Bluetooth;

namespace NfUnitTest1
{
    class TestBleDevice : INativeDevice
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void InitService()
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

        public bool StartAdvertising(bool isDiscoverable, bool isConnectable, byte[] deviceName, ArrayList services)
        {
            throw new NotImplementedException();
        }
    }
}

