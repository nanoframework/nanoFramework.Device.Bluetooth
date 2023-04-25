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
            SignalStrengthFilter = new();
        }

        /// <summary>
        /// Start the BluetoothLEAdvertisementWatcher to scan for Bluetooth LE advertisements.
        /// </summary>
        public void Start()
        {
            // Check and set mode
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.Client);

            _status = BluetoothLEAdvertisementWatcherStatus.Started;

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
            // Check filters
            // Signal strength filter
            if (!_signalStrengthFilter.Filter(args))
            {
                return;
            }

            // Advertisement section Filter (TODO)
            if (_advertisementFilter != null && !_advertisementFilter.Filter(args))
            {
                return;
            }

            Received?.Invoke(this, args);
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