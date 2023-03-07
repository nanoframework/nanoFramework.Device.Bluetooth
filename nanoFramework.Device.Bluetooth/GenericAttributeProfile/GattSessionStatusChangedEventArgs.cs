//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents the SessionStatusChanged event args.
    /// </summary>
    public class GattSessionStatusChangedEventArgs
    {
        private GattSessionStatus _status;
        private BluetoothError _bluetoothError;

        internal GattSessionStatusChangedEventArgs(GattSessionStatus status, BluetoothError bluetoothError)
        {
            _status = status;
            _bluetoothError = bluetoothError;
        }

        /// <summary>
        /// Gets the status of the GATT session.
        /// </summary>
        public GattSessionStatus Status { get => _status; }

        /// <summary>
        /// Gets the error of the GATT session.
        /// </summary>
        public BluetoothError Error { get => _bluetoothError; }
    }
}
