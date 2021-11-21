// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Text;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Writes data to an output stream.
    /// </summary>
    public sealed class DataWriter : Buffer
    {
        /// <summary>
        /// Creates and initializes a new instance of the data writer.
        /// </summary>
        public DataWriter() : base(32)
        {
        }

        /// <summary>
        /// Gets the size of a string.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>The size of the string, in bytes.</returns>
        public uint MeasureString(String value)
        {
            Encoding encoding = Encoding.UTF8;
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return (uint)encoding.GetBytes(value).Length;
        }

        /// <summary>
        /// Writes a Boolean value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteBoolean(Boolean value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a number of bytes from a buffer.
        /// </summary>
        /// <param name="buffer">The value to write.</param>
        public void WriteBuffer(Buffer buffer)
        {
            WriteBuffer(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes a range of bytes from a buffer to the output stream.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The starting byte to be written.</param>
        /// <param name="count">The number of bytes to write.</param>
        public void WriteBuffer(Buffer buffer, uint start, uint count)
        {
            byte[] copyBuffer = new byte[count];
            Array.Copy(buffer.Data, (int)start, copyBuffer, 0, (int)count);

            WriteBytes(copyBuffer);
        }

        /// <summary>
        /// Writes a byte value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteByte(Byte value)
        {
            WriteBytes(new byte[] { value });
        }

        /// <summary>
        /// Writes an array of byte values to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteBytes(Byte[] value)
        {
            EnsureCapacity((uint)(base.Length + value.Length));

            Array.Copy(value, 0, base.Data, (int)base.Length, value.Length);
            base.Length += (uint)value.Length;
        }

        /// <summary>
        /// Writes a date and time value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteDateTime(DateTime value)
        {
            WriteInt64(value.Ticks);
        }

        /// <summary>
        /// Writes a floating-point value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteDouble(Double value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a GUID value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteGuid(Guid value)
        {
            WriteBytes(value.ToByteArray());
        }

        /// <summary>
        /// Writes a 16-bit integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt16(Int16 value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 32-bit integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt32(Int32 value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 64-bit integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteInt64(Int64 value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Write a floating-point value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteSingle(Single value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a string value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <returns>The length of the string.</returns>
        public uint WriteString(String value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            byte[] strBytes = Encoding.UTF8.GetBytes(value);
            WriteBytes(strBytes);
            return (uint)strBytes.Length;
        }

        /// <summary>
        /// Writes a time interval value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteTimeSpan(TimeSpan value)
        {
            WriteInt64(value.Ticks);
        }

        /// <summary>
        /// Writes a 16-bit unsigned integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteUInt16(UInt16 value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 32-bit unsigned integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteUInt32(uint value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes a 64-bit unsigned integer value to the output stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public void WriteUInt64(UInt64 value)
        {
            WriteBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Returns byte buffer of data in buffer
        /// </summary>
        /// <returns>Buffer</returns>
        public Buffer DetachBuffer()
        {
            return (Buffer)this;
        }
    }
}