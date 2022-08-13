//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using nanoFramework.Runtime.Events;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace nanoFramework.Device.Bluetooth
{
    internal class BluetoothEventListener : IEventProcessor, IEventListener
    {
        // Map of Bluetooth Characteristic numbers to GattLocalCharacteristic objects.
        private static readonly ArrayList _characteristicMap = new();

        // Map of Bluetooth connectionHandles to BluetoothLEDevice objects.
        // Used for routing server events
        private static readonly ArrayList _leDeviceMap = new();

        // Reference to current BluetoothLEAdvertisementWatcher for events posting
        private static BluetoothLEAdvertisementWatcher _watcher = null;

        private static AutoResetEvent _watcherEvent = new(false);
        private static Queue _watcherQueue = new();

        public BluetoothEventListener()
        {
            EventSink.AddEventProcessor(EventCategory.Bluetooth, this);
            EventSink.AddEventListener(EventCategory.Bluetooth, this);
        }

        /// <summary>
        /// Reset and remove all linked objects
        /// </summary>
        public void Reset()
        {
            _characteristicMap.Clear();
            _leDeviceMap.Clear();
            _watcher = null;
        }

        public BaseEvent ProcessEvent(uint data1, uint data2, DateTime time)
        {
            BluetoothEventType BLEEventType = (BluetoothEventType)(data1 & 0xff);

            // Data1 is packed by PostManagedEvent, so we need to unpack the high word.
            ushort data1High = (ushort)(data1 >> 16);
            if (BLEEventType < BluetoothEventType.AdvertisementDiscovered)
            {

                return new BluetoothEventClient
                {
                    // Data1, Data2 is packed by PostManagedEvent.
                    //
                    // Data1
                    // DDCC00TT where DD = descriptorId, CC = characteristicId, TT = BluetoothEventType
                    type = BLEEventType,
                    characteristicId = (ushort)((data1 >> 16) & 0x00ff),
                    descriptorId = data1High,
                    id = (ushort)(data2 & 0xffff)
                };
            }
            else if (BLEEventType < BluetoothEventType.ScanningComplete)
            {
                return new BluetoothEventScan
                {
                    type = BLEEventType,
                    id = (ushort)(data2 & 0xffff)
                };
            }
            else
            {
                return new BluetoothEventCentral
                {
                    // Data1, Data2 is packed by PostManagedEvent, so we need to unpack the high word.
                    //
                    // Data1  - HHHH00TT where H=Connection_Handle, TT = BluetoothEventType
                    // Data2  - IICCCCSS where II = serviceHandle(8 bits), CCCCC = characteristicHandle(16 bits), SS = status(8 bits)
                    type = BLEEventType,
                    connectionHandle = data1High,
                    serviceHandle = (ushort)((data2 >> 24) & 0xff),
                    characteristicHandle = (ushort)((data2 >> 8) & 0xffff),
                    status = (ushort)(data2 & 0x00ff)
                };
            }
        }

        public void InitializeForEventSource()
        {
            // start watcher thread
            new Thread(WatcherEventQueueThread).Start();
        }

        private bool OnEventClient(BluetoothEventClient btEvent)
        {
            GattLocalCharacteristic lc = null;

            lock (_characteristicMap)
            {
                // Search for Characteristic using Characteristic id part of Id
                lc = FindCharacteristic(btEvent.characteristicId);
            }

            // Avoid calling this under a lock to prevent a potential lock inversion.
            if (lc != null)
            {
                switch (btEvent.type)
                {
                    case BluetoothEventType.Read:
                        lc.OnReadRequested(btEvent.descriptorId, new GattReadRequestedEventArgs(btEvent.id, null));
                        break;

                    case BluetoothEventType.Write:
                        lc.OnWriteRequested(btEvent.descriptorId, new GattWriteRequestedEventArgs(btEvent.id, null));
                        break;

                    case BluetoothEventType.ClientSubscribed:
                        {
                            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(btEvent.id));
                            GattSubscribedClient sc = new(gs);
                            lc.OnSubscribedClientsChanged(true, sc);
                        }
                        break;

                    case BluetoothEventType.ClientUnsubscribed:
                        {
                            GattSession gs = GattSession.FromDeviceId(new BluetoothDeviceId(btEvent.id));
                            GattSubscribedClient sc = new(gs);
                            lc.OnSubscribedClientsChanged(false, sc);
                        }
                        break;
                }
            }

            return true;

        }

        private bool OnEventScan(BluetoothEventScan btEvent)
        {
            // Process Watcher events on separate thread so events for central don't
            // get held up when trying to connect to device within a watcher event
            lock(_watcherQueue)
            {
                _watcherQueue.Enqueue(btEvent);
            }
            _watcherEvent.Set();
            return true;
        }

        private bool OnEventCentral(BluetoothEventCentral btEvent)
        {
            // Need to route to correct BluetoothLEDevice
            BluetoothLEDevice ledev = FindLeDevice(btEvent.connectionHandle);
            ledev?.OnEvent(btEvent);

            return true;
        }

        public bool OnEvent(BaseEvent ev)
        {
            if (ev.GetType() == typeof(BluetoothEventClient))
            {
                return OnEventClient((BluetoothEventClient)ev);
            }
            else if (ev.GetType() == typeof(BluetoothEventScan))
            {
                return OnEventScan((BluetoothEventScan)ev);
            }
            else if (ev.GetType() == typeof(BluetoothEventCentral))
            {
                return OnEventCentral((BluetoothEventCentral)ev);
            }

            // Not handled
            return false;
        }

        internal BluetoothLEAdvertisementWatcher Watcher { get => _watcher; set => _watcher = value; }

        #region Central/Client routing
        public void AddLeDevice(BluetoothLEDevice d)
        {
            lock (_leDeviceMap)
            {
                if (!_leDeviceMap.Contains(d))
                {
                    _leDeviceMap.Add(d);
                }
                else
                {
                }
            }
        }
        public void RemoveLeDevice(BluetoothLEDevice d)
        {
            lock (_leDeviceMap)
            {
                if (_leDeviceMap.Contains(d))
                {
                    _leDeviceMap.Remove(d);
                }
            }
        }

        public BluetoothLEDevice FindLeDevice(ushort connectionHandle)
        {
            for (int i = 0; i < _leDeviceMap.Count; i++)
            {
                if (((BluetoothLEDevice)_leDeviceMap[i]).ConnectionHandle == connectionHandle)
                {
                    return (BluetoothLEDevice)_leDeviceMap[i];
                }
            }

            return null;
        }

        public int LeDeviceCount { get => _leDeviceMap.Count;  }
        #endregion

        #region Server characteristic routing
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

        // Run event on separate thread so as not to block event queue
        // This will allow connects to devices in the event thread and they don't just time out
        private void WatcherEventQueueThread()
        {
            BluetoothEventScan btEvent;
            bool run = true; // added to keep Sonar happy

            while (run)
            {
                if (_watcherEvent.WaitOne())
                {
                    while(_watcherQueue.Count > 0)
                    {
                        lock (_watcherQueue)
                        {
                            btEvent = (BluetoothEventScan)_watcherQueue.Dequeue();
                        }

                        switch (btEvent.type)
                        {
                            case BluetoothEventType.AdvertisementDiscovered:
                                BluetoothLEAdvertisementReceivedEventArgs eventRxArgs = BluetoothLEAdvertisementReceivedEventArgs.CreateFromEvent(btEvent.id);
                                eventRxArgs.Timestamp = DateTime.UtcNow;
                                _watcher?.OnReceived(eventRxArgs);

                                break;

                            case BluetoothEventType.ScanningComplete:
                                BluetoothLEAdvertisementWatcherStoppedEventArgs eventStArgs = new((BluetoothError)btEvent.id);
                                _watcher?.OnStopped(eventStArgs);
                                break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}

