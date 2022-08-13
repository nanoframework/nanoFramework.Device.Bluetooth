//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//
using System;

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

        /// <summary>
        /// Create a new BluetoothSignalStrengthFilter object.
        /// </summary>
        public BluetoothSignalStrengthFilter()
        {
            InRangeThresholdInDBm = -128;
            OutOfRangeThresholdInDBm = -128;
            OutOfRangeTimeout = new TimeSpan(0, 0, 10); // seconds
        }

        ///// <summary>
        ///// The interval at which received signal strength indicator (RSSI) events are sampled.
        ///// </summary>
        //public TimeSpan? SamplingInterval { get; set; }

        /// <summary>
        /// The time out for a received signal strength indicator (RSSI) event to be considered
        /// out of range.
        /// </summary>
        public TimeSpan OutOfRangeTimeout { get => _outOfRangeTimeout; set => _outOfRangeTimeout = value; }

        /// <summary>
        /// The minimum received signal strength indicator (RSSI) value in dBm on which RSSI
        /// events will be considered out of range.
        /// </summary>
        public short OutOfRangeThresholdInDBm { get => _outOfRangeThresholdInDBm; set => _outOfRangeThresholdInDBm = value; }

        /// <summary>
        /// The minimum received signal strength indicator (RSSI) value in dBm on which RSSI
        /// events will be propagated or considered in range if the previous events were
        /// considered out of range.
        /// </summary>
        public short InRangeThresholdInDBm { get => _inRangeThresholdInDBm; set => _inRangeThresholdInDBm = value; }
    }
}