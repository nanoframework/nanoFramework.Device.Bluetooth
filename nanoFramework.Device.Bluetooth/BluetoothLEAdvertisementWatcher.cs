//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// A class to receive Bluetooth Low Energy (LE) advertisements.
    /// </summary>
    public class BluetoothLEAdvertisementWatcher
    {
        private BluetoothLEAdvertisementWatcherStatus _status;
        private BluetoothLEScanningMode _scanningMode;
        private BluetoothLEAdvertisementFilter _advertisementFilter;
        private BluetoothSignalStrengthFilter _signalStrengthFilter;

        Timer _scanCheck;
        Hashtable _scanResults = new();
        Object _scanResultsLock = new Object();

        private class ScanItem
        {
            public short rssi;  // RSSI of last scan
            public bool  inRange; // True if device was in range.
            public DateTime outRangeTime;  // Time device went out of range
            public bool active;     // Scan item active, used for purging entries
        }

        /// <summary>
        /// Delegate for new Bluetooth LE advertisement events received.
        /// </summary>
        /// <param name="sender">BluetoothLEAdvertisementWatcher sending event</param>
        /// <param name="args">Event arguments</param>
        public delegate void BluetoothLEAdvertisementReceivedHandler(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs args);

        /// <summary>
        /// Delegate for new Bluetooth LE advertisement stop events.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">Event arguments</param>
        public delegate void BluetoothLEAdvertisementStoppedEvenHandler(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args);

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementWatcher object.
        /// </summary>
        public BluetoothLEAdvertisementWatcher() : this(new BluetoothLEAdvertisementFilter())
        {
        }

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementWatcher object with an advertisement filter
        /// to initialize the watcher.
        /// </summary>
        /// <param name="advertisementFilter">The advertisement filter to initialize the watcher.</param>
        public BluetoothLEAdvertisementWatcher(BluetoothLEAdvertisementFilter advertisementFilter)
        {
            _status = BluetoothLEAdvertisementWatcherStatus.Created;
            _advertisementFilter = advertisementFilter;
            SignalStrengthFilter = new BluetoothSignalStrengthFilter();
        }

        /// <summary>
        /// Start the BluetoothLEAdvertisementWatcher to scan for Bluetooth LE advertisements.
        /// </summary>
        public void Start()
        {
            // Check and set mode
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.Client);

            _status = BluetoothLEAdvertisementWatcherStatus.Started;
            _scanResults = new Hashtable();

            NativeStartAdvertisementWatcher((int)_scanningMode);
            BluetoothLEServer._bluetoothEventManager.Watcher = this;

            // Remove inactive entries from scan results every 5 minutes 
            TimeSpan time = new TimeSpan(0, 5, 0);
            _scanCheck = new Timer(scanCheckCallback, 0, time, time);
        }

        /// <summary>
        /// Called every 5 minutes to purge scan hash.
        /// </summary>
        /// <param name="state"></param>
        private void scanCheckCallback(object state)
        {
            lock (_scanResultsLock)
            {
                ArrayList removeList = new ArrayList();

                foreach (DictionaryEntry item in _scanResults)
                {
                    if (!((ScanItem)item.Value).active)
                    {
                        removeList.Add(item.Key);
                    }
                    else
                    {
                        ((ScanItem)item.Value).active = false;
                    }
                }

                // Now delete expired items
                foreach (ulong addressKey in removeList)
                {
                    DeleteScanEntry(addressKey);
                }
            }
        }

        /// <summary>
        /// Stop the BluetoothLEAdvertisementWatcher and disable the scanning for Bluetooth
        /// LE advertisements.
        /// </summary>
        public void Stop()
        {
            _scanCheck.Dispose();

            _status = BluetoothLEAdvertisementWatcherStatus.Stopping;

            BluetoothLEServer._bluetoothEventManager.Watcher = null;

            NativeStopAdvertisementWatcher();

            _status = BluetoothLEAdvertisementWatcherStatus.Stopped;

            BluetoothLEDevice.IdleOnLastConnection();
        }

        /// <summary>
        /// Gets or sets a BluetoothSignalStrengthFilter object used for configuration of
        /// Bluetooth LE advertisement filtering that uses signal strength-based filtering.
        /// </summary>
        public BluetoothSignalStrengthFilter SignalStrengthFilter { get => _signalStrengthFilter; set => _signalStrengthFilter = value; }

        /// <summary>
        /// Gets or sets the Bluetooth LE scanning mode.
        /// </summary>
        public BluetoothLEScanningMode ScanningMode { get => _scanningMode; set => _scanningMode = value; }

        /// <summary>
        /// Gets or sets a BluetoothLEAdvertisementFilter object used for configuration of
        /// Bluetooth LE advertisement filtering that uses payload section-based filtering.
        /// </summary>
        public BluetoothLEAdvertisementFilter AdvertisementFilter { get => _advertisementFilter; set => _advertisementFilter = value; }

        /// <summary>
        /// Gets the current status of the BluetoothLEAdvertisementWatcher.
        /// </summary>
        public BluetoothLEAdvertisementWatcherStatus Status { get => _status; }

        internal void OnReceived(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            lock (_scanResultsLock)
            {
                // Check Scan in RSSI range
                if (ScanRssiFilter(args))
                {
                    Received?.Invoke(this, args);
                }
            }
        }

        /// <summary>
        /// Check Event against RSSI filter
        /// </summary>
        /// <param name="args">BluetoothLEAdvertisementReceivedEventArgs</param>
        /// <returns>Returns False to ignore event</returns>
        internal bool ScanRssiFilter(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            ScanItem scan = FindScanEntry(args.BluetoothAddress);
            bool inRange = (args.RawSignalStrengthInDBm >= SignalStrengthFilter.InRangeThresholdInDBm);
            if (scan == null && inRange)
            {
                // New entry and in range then add it to list
                scan = AddOrReplaceScanEntry(args.BluetoothAddress, args.RawSignalStrengthInDBm, inRange);
            }

            if (scan != null)
            {
                scan.active = true;  // flag entry as active so not purged

                if (!inRange)
                {
                    if (args.RawSignalStrengthInDBm < SignalStrengthFilter.OutOfRangeThresholdInDBm)
                    {
                        // Completely out of range, ignore
                        DeleteScanEntry(args.BluetoothAddress);
                        return false;
                    }
                    
                    // In between in and out of range thresholds, check time out of range
                    if (scan.inRange)
                    {
                        // If previously in range and now out
                        // then set date time for time out of range
                        AddOrReplaceScanEntry(args.BluetoothAddress, args.RawSignalStrengthInDBm, inRange);
                    }
                    else 
                    {
                        // If previously moved out of range then check timer and still out of range.
                        if ((scan.outRangeTime + SignalStrengthFilter.OutOfRangeTimeout) < DateTime.UtcNow )
                        {
                            // Moved out of range for time out period
                            // Remove scan
                            DeleteScanEntry(args.BluetoothAddress);
                            return false;
                        }
                    }
                }

                return true;
            }
            return false;
        }

        private ScanItem FindScanEntry(UInt64 address)
        {
            if (_scanResults.Contains(address))
            {
                return (ScanItem)_scanResults[address];
            }
            return null;
        }

        private ScanItem AddOrReplaceScanEntry(UInt64 address, short Rssi, bool InRange)
        {
            ScanItem item = new()
            {
                rssi = Rssi,
                inRange = InRange,
                active = true
            };

            if (!InRange)
            {
                // It out of range now, save time for time out checking 
                item.outRangeTime = DateTime.UtcNow;
            }

            DeleteScanEntry(address);

            _scanResults.Add(address, item);

            return item;
        }

        private void DeleteScanEntry(UInt64 address)
        {
            if (_scanResults.Contains(address))
            {
                _scanResults.Remove(address);
            }
        }
        /// <summary>
        ///  Notification for new Bluetooth LE advertisement events received.
        /// </summary>
        public event BluetoothLEAdvertisementReceivedHandler Received;

        /// <summary>
        /// Notification to the application that the Bluetooth LE scanning for advertisements has
        /// been canceled or aborted either by the application or due to an error.
        /// </summary>
        public event BluetoothLEAdvertisementStoppedEvenHandler Stopped;

        internal void OnStopped(BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            Stopped?.Invoke(this, args);
        }

        #region Native
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStartAdvertisementWatcher(int mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeStopAdvertisementWatcher();
        #endregion

    }
}