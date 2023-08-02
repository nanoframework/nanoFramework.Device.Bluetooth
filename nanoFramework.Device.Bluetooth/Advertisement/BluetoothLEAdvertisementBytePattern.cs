//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// A Bluetooth LE advertisement byte pattern for filters to match.
    /// </summary>
    public class BluetoothLEAdvertisementBytePattern
    {
        private byte _dataType;
        private Buffer _data;
        private short _offset;

        /// <summary>
        /// Create a new BluetoothLEAdvertisementBytePattern object.
        /// </summary>
        public BluetoothLEAdvertisementBytePattern() : this(0, 0, null)
        {
        }

        /// <summary>
        /// Create a new <see cref="BluetoothLEAdvertisementBytePattern"/> object with an advertisement data type to match, the
        /// advertisement data byte pattern to match, and the offset of the byte pattern from the beginning of the advertisement
        /// data section.
        /// </summary>
        /// <param name="dataType">The Bluetooth LE advertisement data type to match.</param>
        /// <param name="offset">The offset of byte pattern from beginning of advertisement data section.</param>
        /// <param name="data">The Bluetooth LE advertisement data byte pattern to match.</param>
        public BluetoothLEAdvertisementBytePattern(byte dataType, short offset, Buffer data)
        {
            _dataType = dataType;
            _offset = offset;
            _data = data;
        }

        /// <summary>
        /// The Bluetooth LE advertisement data type defined by the Bluetooth Special Interest Group (SIG) to match.
        /// </summary>
        public byte DataType { get => _dataType; set => _dataType = value; }

        /// <summary>
        /// The Bluetooth LE advertisement data byte pattern to match.
        /// </summary>
        public Buffer Data { get => _data; set => _data = value; }

        /// <summary>
        /// The offset of byte pattern from beginning of advertisement data section.
        /// </summary>
        public short Offset { get => _offset; set => _offset = value; }
    }
}
