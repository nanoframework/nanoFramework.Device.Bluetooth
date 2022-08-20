//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// A Bluetooth LE advertisement section. A Bluetooth LE advertisement packet can
    /// contain multiple instances of these BluetoothLEAdvertisementDataSection objects.
    /// </summary>
    public class BluetoothLEAdvertisementDataSection
    {
        private byte _dataType;
        private Buffer _buffer;

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementDataSection object.
        /// </summary>
        public BluetoothLEAdvertisementDataSection() : this(0, new Buffer(16))
        {
        }

        /// <summary>
        /// Creates a new BluetoothLEAdvertisementDataSection object with the Bluetooth LE
        /// advertisement data type and the payload.
        /// </summary>
        /// <param name="dataType">The Bluetooth LE advertisement data type as defined by the Bluetooth Special Interest Group (SIG).</param>
        /// <param name="data">The Bluetooth LE advertisement data payload.</param>
        public BluetoothLEAdvertisementDataSection(byte dataType, Buffer data)
        {
            _dataType = dataType;
            _buffer = data;
        }

        /// <summary>
        /// The Bluetooth LE advertisement data type as defined by the Bluetooth Special
        /// Interest Group (SIG).
        /// </summary>
        public byte DataType { get => _dataType; set => _dataType = value; }

        /// <summary>
        /// The Bluetooth LE advertisement data payload.
        /// </summary>
        public Buffer Data { get => _buffer; set => _buffer = value; }
    }
}