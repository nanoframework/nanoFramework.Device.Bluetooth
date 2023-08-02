//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Provides data for a StatusChanged event on a <see cref="BluetoothLEAdvertisementPublisher"/>.
    /// </summary>
    public class BluetoothLEAdvertisementPublisherStatusChangedEventArgs
    {
        private readonly BluetoothError _error;
        private readonly BluetoothLEAdvertisementPublisherStatus _status;
        private readonly short _selectedTransmitPowerLevelInDBm;

        internal BluetoothLEAdvertisementPublisherStatusChangedEventArgs(BluetoothLEAdvertisementPublisherStatus status, BluetoothError error, short selectedTransmitPowerLevelInDBm)
        {
            _status = status;
            _error = error;
            _selectedTransmitPowerLevelInDBm = selectedTransmitPowerLevelInDBm;
        }

        /// <summary>
        /// Gets the error status for a StatusChanged event on a <see cref="BluetoothLEAdvertisementPublisher"/>.
        /// </summary>
        public BluetoothError Error { get => _error; }

        /// <summary>
        /// Gets the new status of the <see cref="BluetoothLEAdvertisementPublisher"/>.
        /// </summary>
        public BluetoothLEAdvertisementPublisherStatus Status { get => _status; }

        /// <summary>
        /// The current transmit power selected. 
        /// If the Extended Advertisement format is not supported by the adapter, this instead represents the adapter's default transmit power level.
        /// </summary>
        public short SelectedTransmitPowerLevelInDBm { get => _selectedTransmitPowerLevelInDBm; }
    }
}
