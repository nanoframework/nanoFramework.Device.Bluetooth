//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using nanoFramework.Device.Bluetooth.Advertisement;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// A class to receive Bluetooth Low Energy (LE) advertisements.
    /// </summary>
    public class BluetoothLEAdvertisementWatcher
    {
        BluetoothLEAdvertisementWatcherStatus _status;
        BluetoothLEScanningMode _scanningMode;
        BluetoothLEAdvertisementFilter _advertisementFilter;
        BluetoothSignalStrengthFilter _signalStrengthFilter;

        Hashtable _scanResults = new();

        private class ScanItem
        {
            public short rssi;
            public bool  inRange;
            public DateTime outRangeTime;
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
        }

        /// <summary>
        /// Stop the BluetoothLEAdvertisementWatcher and disable the scanning for Bluetooth
        /// LE advertisements.
        /// </summary>
        public void Stop()
        {
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
            // Check Scan in RSSI range
            if (ScanRssiFilter(args))
            {
                Received?.Invoke(this, args);
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
                        // If previously in range and not out
                        // then set date time for time out
                        AddOrReplaceScanEntry(args.BluetoothAddress, args.RawSignalStrengthInDBm, inRange);
                    }
                    else 
                    {
                        // If previously moved out of range then check timer
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
        /// been cancelled or aborted either by the application or due to an error.
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