//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

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

        /// <summary>
        /// Returns a byte array formatted with data section data in format used for adverts.
        /// 1 byte length, 1 byte type, bytes data
        /// </summary>
        /// <returns>Byte array for advert section.</returns>
        internal byte[] ToAdvertisentBytes()
        {
            byte[] data = new byte[_buffer.Length + 2];

            data[0] = (byte)(_buffer.Length + 1);
            data[1] = _dataType;
            Array.Copy(_buffer.Data, 0, data, 2, (int)_buffer.Length);

            return data;
        }

        /// <summary>
        /// Returns length of advertisement bytes.
        /// </summary>
        /// <returns></returns>
        internal int AdvertisentLength() 
        {
            return (int)_buffer.Length + 2;
        }
    }
}