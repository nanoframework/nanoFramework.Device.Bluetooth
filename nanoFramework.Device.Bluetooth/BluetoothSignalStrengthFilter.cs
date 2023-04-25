//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//
using nanoFramework.Device.Bluetooth.Advertisement;
using System;
using System.Collections;
using System.Threading;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Groups parameters used to configure received signal strength indicator (RSSI)-based
    /// filtering.
    /// </summary>
    public class BluetoothSignalStrengthFilter 
    {
        private short _inRangeThresholdInDBm;
        private short _outOfRangeThresholdInDBm;
        private TimeSpan _outOfRangeTimeout;

        private Timer _scanCheck;
        private Hashtable _scanResults = new();
        private Object _scanResultsLock = new Object();

        private const int DefaultDBM = -127;
        private const int DefaultOorTimeout = 60;

        private class ScanItem
        {
            public short Rssi;  // RSSI of last scan
            public bool InRange; // True if device was in range.
            public DateTime OutRangeTime;  // Time device went out of range
            public bool Active;     // Scan item Active, used for purging entries
        }

        /// <summary>
        /// Create a new BluetoothSignalStrengthFilter object.
        /// </summary>
        public BluetoothSignalStrengthFilter()
        {
            InRangeThresholdInDBm = DefaultDBM;
            OutOfRangeThresholdInDBm = DefaultDBM;
            OutOfRangeTimeout = new TimeSpan(0, 0, DefaultOorTimeout); // seconds

            _scanResults = new Hashtable();

            // Remove inactive entries from scan results every 1 minute 
            TimeSpan time = new TimeSpan(0, 1, 0);
            _scanCheck = new Timer(ScanCheckCallback, 0, time, time);
        }

        ///// <summary>
        ///// The interval at which received signal strength indicator (RSSI) events are sampled.  (TODO)
        ///// </summary>
        //public TimeSpan? SamplingInterval { get; set; }

        /// <summary>
        /// The time out for a received signal strength indicator (RSSI) event to be considered
        /// out of range. Value between 1 and 60 seconds.
        /// </summary>
        public TimeSpan OutOfRangeTimeout { get => _outOfRangeTimeout; set => _outOfRangeTimeout = value; }

        /// <summary>
        /// The minimum received signal strength indicator (RSSI) value in dBm on which RSSI
        /// events will be considered out of range. Value between +20 and -127. Default vale is -127.
        /// </summary>
        public short OutOfRangeThresholdInDBm { get => _outOfRangeThresholdInDBm; set => _outOfRangeThresholdInDBm = value; }

        /// <summary>
        /// The minimum received signal strength indicator (RSSI) value in dBm on which RSSI
        /// events will be propagated or considered in range if the previous events were
        /// considered out of range. Value between +20 and -127. Default vale is -127.
        /// </summary>
        public short InRangeThresholdInDBm { get => _inRangeThresholdInDBm; set => _inRangeThresholdInDBm = value; }

        /// <summary>
        /// Signal strength filter (RSSI).
        /// </summary>
        /// <param name="args">BluetoothLEAdvertisementReceivedEventArgs</param>
        /// <returns>Returns False to ignore event</returns>
        internal bool Filter(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // If default setting then just accept all events
            if (InRangeThresholdInDBm == DefaultDBM)
            {
                return true;
            }

            ScanItem scan = FindScanEntry(args.BluetoothAddress);
            bool inRange = (args.RawSignalStrengthInDBm >= InRangeThresholdInDBm);
            if (scan == null && inRange)
            {
                // New entry and in range then add it to list
                scan = AddOrReplaceScanEntry(args.BluetoothAddress, args.RawSignalStrengthInDBm, inRange);
            }

            if (scan != null)
            {
                scan.Active = true;  // flag entry as active so not purged

                if (!inRange)
                {
                    if (args.RawSignalStrengthInDBm < OutOfRangeThresholdInDBm)
                    {
                        // Completely out of range, ignore
                        DeleteScanEntry(args.BluetoothAddress);
                        return false;
                    }

                    // In between in and out of range thresholds, check time out of range
                    if (scan.InRange)
                    {
                        // If previously in range and now out
                        // then set date time for time out of range
                        AddOrReplaceScanEntry(args.BluetoothAddress, args.RawSignalStrengthInDBm, inRange);
                    }
                    else
                    {
                        // If previously moved out of range then check timer and still out of range.
                        if ((scan.OutRangeTime + OutOfRangeTimeout) < DateTime.UtcNow)
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

        /// <summary>
        /// Called every 5 minutes to purge scan hash.
        /// </summary>
        /// <param name="state"></param>
        private void ScanCheckCallback(object state)
        {
            lock (_scanResultsLock)
            {
                ArrayList removeList = new ArrayList();

                foreach (DictionaryEntry item in _scanResults)
                {
                    if (!((ScanItem)item.Value).Active)
                    {
                        removeList.Add(item.Key);
                    }
                    else
                    {
                        ((ScanItem)item.Value).Active = false;
                    }
                }

                // Now delete expired items
                foreach (ulong addressKey in removeList)
                {
                    DeleteScanEntry(addressKey);
                }
            }
        }

        private ScanItem FindScanEntry(UInt64 address)
        {
            if (_scanResults.Contains(address))
            {
                return (ScanItem)_scanResults[address];
            }

            return null;
        }

        private ScanItem AddOrReplaceScanEntry(UInt64 address, short rssi, bool inRange)
        {
            ScanItem item = new()
            {
                Rssi = rssi,
                InRange = inRange,
                Active = true
            };

            if (!inRange)
            {
                // It out of range now, save time for time out checking 
                item.OutRangeTime = DateTime.UtcNow;
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
    }
}