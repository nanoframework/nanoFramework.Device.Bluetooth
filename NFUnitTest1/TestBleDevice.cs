//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using nanoFramework.Device.Bluetooth;

namespace NfUnitTest1
{
    class TestBleDevice : INativeDevice
    {
        public Void Dispose()
        {
            throw new NotImplementedException();
        }

        public Void InitService()
        {
            throw new NotImplementedException();
        }

        public Void StopAdvertising()
        {
            throw new NotImplementedException();
        }

        public Void WriteRespondWithProtocolError(ushort eventId, Byte protocolError)
        {
            throw new NotImplementedException();
        }

        public Void WriteRespond(ushort eventId)
        {
            throw new NotImplementedException();
        }

        public Byte[] WriteGetData(ushort eventId)
        {
            throw new NotImplementedException();
        }

        public Void ReadRespondWithProtocolError(ushort eventId, byte otherError)
        {
            throw new NotImplementedException();
        }

        public Void ReadRespondWithValue(ushort eventId, byte[] data)
        {
            throw new NotImplementedException();
        }

        public Int32 NotifyClient(ushort connection, ushort characteristicId, byte[] notifyBuffer)
        {
            throw new NotImplementedException();
        }

        public Boolean StartAdvertising(bool isDiscoverable, bool isConnectable, byte[] deviceName, ArrayList services)
        {
            throw new NotImplementedException();
        }
    }
}

