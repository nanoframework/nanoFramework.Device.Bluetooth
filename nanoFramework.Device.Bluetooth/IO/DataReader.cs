//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Reads data from an input buffer.
    /// </summary>
    public sealed class DataReader 
    {
        private Buffer _buffer;
        private int _currentReadPosition;

        const int defaultBufferSize = 32;

        private DataReader(Buffer buffer)
        {
            _buffer = buffer;
            _currentReadPosition = 0;
        }

        /// <summary>
        /// Creates a new instance of the data reader with data from the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>The data reader.</returns>
        public static DataReader FromBuffer(Buffer buffer)
        {
            return new DataReader(buffer);
        }

        /// <summary>
        /// Gets the size of the buffer that has not been read.
        /// </summary>
        /// <value>
        /// The size of the buffer that has not been read, in bytes.
        /// </value>
        public uint UnconsumedBufferLength { get { return (_buffer.Length - (uint)_currentReadPosition); } }


        /// <summary>
        /// Reads a Boolean value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public bool ReadBoolean()
        {
            var value = _buffer.Data[IncreaseReadPosition(1)] > 0;

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a buffer from the input buffer.
        /// </summary>
        /// <param name="length">The length of the buffer, in bytes.</param>
        /// <returns>The buffer.</returns>
        public Buffer ReadBuffer(UInt32 length)
        {
            Buffer buffer = new Buffer(length);

            Array.Copy(_buffer.Data, IncreaseReadPosition((int)length), buffer.Data, 0, (int)length);

            CheckReadPosition();

            return buffer;
        }

        /// <summary>
        /// Reads a byte value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public byte ReadByte()
        {
            var value = _buffer.Data[IncreaseReadPosition(1)];

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads an array of byte values from the input buffer.
        /// </summary>
        /// <param name="value">The array of values.</param>
        public void ReadBytes(Byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("Argument is null");
            }

            Array.Copy(_buffer.Data, IncreaseReadPosition(value.Length), value, 0, value.Length);

            CheckReadPosition();
        }

        /// <summary>
        /// Reads a date and time value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public DateTime ReadDateTime()
        {
            // read position update and check are performed on the call
            return new DateTime(ReadInt64());
        }

        /// <summary>
        /// Reads a floating-point value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public double ReadDouble()
        {
            var value = BitConverter.ToDouble(_buffer.Data, IncreaseReadPosition(8));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a GUID value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public Guid ReadGuid()
        {
            byte[] byteArray = new byte[16];

            // read position update and check are performed on the call
            ReadBytes(byteArray);

            return new Guid(byteArray);
        }

        /// <summary>
        /// Reads a 16-bit integer value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public short ReadInt16()
        {
            var value = BitConverter.ToInt16(_buffer.Data, IncreaseReadPosition(2));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a 32-bit integer value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public int ReadInt32()
        {
            var value = BitConverter.ToInt32(_buffer.Data, IncreaseReadPosition(4));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a 64-bit integer value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public long ReadInt64()
        {
            var value = BitConverter.ToInt64(_buffer.Data, IncreaseReadPosition(8));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a floating-point value from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public float ReadSingle()
        {
            var value = BitConverter.ToSingle(_buffer.Data, IncreaseReadPosition(4));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a string value from the input buffer.
        /// </summary>
        /// <param name="codeUnitCount">The length of the string.</param>
        /// <returns>The value.</returns>
        public string ReadString(UInt32 codeUnitCount)
        {
            Char[] buffer = new Char[codeUnitCount];

            int readPosition = IncreaseReadPosition((int)codeUnitCount);

            Encoding.UTF8.GetDecoder().Convert(_buffer.Data, readPosition, (int)codeUnitCount, buffer, 0, (int)codeUnitCount, false, out Int32 bytesUsed, out Int32 charsUsed, out Boolean completed);
            var value = new String(buffer, 0, charsUsed);

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a time interval from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public TimeSpan ReadTimeSpan()
        {
            // read position update and check are performed on the call
            return new TimeSpan(ReadInt64());
        }

        /// <summary>
        /// Reads a 16-bit unsigned integer from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public ushort ReadUInt16()
        {
            var value = BitConverter.ToUInt16(_buffer.Data, IncreaseReadPosition(2));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a 32-bit unsigned integer from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public uint ReadUInt32()
        {
            var value = BitConverter.ToUInt32(_buffer.Data, IncreaseReadPosition(4));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Reads a 64-bit unsigned integer from the input buffer.
        /// </summary>
        /// <returns>The value.</returns>
        public ulong ReadUInt64()
        {
            var value = BitConverter.ToUInt64(_buffer.Data, IncreaseReadPosition(8));

            CheckReadPosition();

            return value;
        }

        /// <summary>
        /// Increases the backing buffer read position.
        /// </summary>
        /// <param name="count">How many bytes to read from the backing buffer.</param>
        /// <returns>
        /// The current buffer position before increasing it by <para>count</para>.
        /// </returns>
        private int IncreaseReadPosition(int count)
        {
            if (UnconsumedBufferLength < count)
            {
                throw new ArgumentOutOfRangeException("count","No data in buffer");
            }

            // save current read position
            int newPosition = _currentReadPosition;

            // increase by count request
            _currentReadPosition += count;

            return newPosition;
        }

        /// <summary>
        /// Checks current read position and resets the backing buffer if all bytes have been read
        /// </summary>
        private void CheckReadPosition()
        {
            if (_currentReadPosition == _buffer.Length)
            {
                _buffer = new Buffer(defaultBufferSize);
                _currentReadPosition = 0;
            }
        }
    }
}
