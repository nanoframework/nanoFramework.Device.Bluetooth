//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Diagnostics;

using nanoFramework.Runtime.Events;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth
{
    internal class BluetoothEventListener : IEventProcessor, IEventListener
    {
        // Map of Bluetooth Characteristic numbers to GattLocalCharacteristic objects.
        private static readonly ArrayList _characteristicMap = new ArrayList();

        public BluetoothEventListener()
        {
            EventSink.AddEventProcessor(EventCategory.Bluetooth, this);
            EventSink.AddEventListener(EventCategory.Bluetooth, this);
        }

        public BaseEvent ProcessEvent(uint data1, uint data2,  DateTime time)
        {
            return new BluetoothEvent
            {
                // Data1, Data2 is packed by PostManagedEvent, so we need to unpack the high word.
                //
                // Data1
                // DDCC00TT where DDCC = descriptorId, CC = characteristicId, TT = BluetoothEventType
                type = (BluetoothEventType)(data1 & 0xff),
                characteristicId = (ushort)((data1 >> 16) & 0x00ff),
                descriptorId = (ushort)(data1 >> 16),
                ID = (ushort)(data2 & 0xffff)
            };
        }

        public void InitializeForEventSource()
        {
            //Nothing to Initialise
        }

        public bool OnEvent(BaseEvent ev)
        {
            var btEvent = (BluetoothEvent)ev;
            GattLocalCharacteristic lc = null;

            lock (_characteristicMap)
            {
                // Search for Characteristic using Characteristic ID part of Id
                lc = FindCharacteristic(btEvent.characteristicId);
            }

            // Avoid calling this under a lock to prevent a potential lock inversion.
            if (lc != null)
            {
                switch (btEvent.type)
                {
                    case BluetoothEventType.Read:
                        lc.OnReadRequested(btEvent.descriptorId, new GattReadRequestedEventArgs(btEvent.ID, null));
                        break;

                    case BluetoothEventType.Write:
                        lc.OnWriteRequested(btEvent.descriptorId, new GattWriteRequestedEventArgs(btEvent.ID, null));
                        break;

                    case BluetoothEventType.ClientSubscribed:
                        {
                            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(btEvent.ID));
                            GattSubscribedClient sc = new GattSubscribedClient(gs);
                            lc.OnSubscribedClientsChanged(true, sc);
                        }
                        break;

                    case BluetoothEventType.ClientUnsubscribed:
                        {
                            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(btEvent.ID));
                            GattSubscribedClient sc = new GattSubscribedClient(gs);
                            lc.OnSubscribedClientsChanged(false, sc);
                        }
                        break;
                }
            }            

            return true;
        }

        public void AddCharacteristic(GattLocalCharacteristic c)
        {
            lock (_characteristicMap)
            {
                _characteristicMap.Add(c);
            }
        }

        public void RemoveCharacteristic(GattLocalCharacteristic c)
        {
            lock (_characteristicMap)
            {
                var fc = FindCharacteristic(c._characteristicId);
                if (fc != null)
                {
                    _characteristicMap.Remove(fc);
                }
            }
        }

        private GattLocalCharacteristic FindCharacteristic(ushort id)
        {
            for (int i = 0; i < _characteristicMap.Count; i++)
            {
                if (((GattLocalCharacteristic)_characteristicMap[i])._characteristicId == id)
                {
                    return (GattLocalCharacteristic)_characteristicMap[i];
                }
            }

            return null;
        }
    }
}

