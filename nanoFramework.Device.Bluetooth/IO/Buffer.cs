using System;
using System.Text;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Represents a referenced array of bytes used by byte stream read and write interfaces.
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// Byte buffer
        /// </summary>
        private byte[] _buffer;

        /// <summary>
        /// Length of data in byte buffer.
        /// </summary>
        protected uint _length;

        /// <summary>
        /// Constructor for Buffer with a specific capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public Buffer(uint capacity)
        {
            _buffer = new byte[capacity];
            _length = 0;
        }

        /// <summary>
        /// Constructor for Buffer with an external byte buffer.
        /// </summary>
        /// <param name="array"></param>
        public Buffer(byte[] array)
        {
            _buffer = array;
            _length = (uint)array.Length;
        }

        /// <summary>
        /// Gets the maximum number of bytes that the buffer can hold.
        /// </summary>
        public uint Capacity { get => (uint)_buffer.Length; }

        /// <summary>
        /// Gets the number of bytes currently in use in the buffer.
        /// </summary>
        public uint Length
        {
            get => _length;
            set
            {
                if (value > _buffer.Length || value < 0)
                {
                    throw new ArgumentException("Length greater than current buffer");
                }

                _length = value;
            }
        }

        /// <summary>
        /// Ensure buffer has a certain capacity. If to small it will be expanded
        /// </summary>
        /// <param name="newCapacity">New capacity required</param>
        public void EnsureCapacity(uint newCapacity)
        {
            if (newCapacity > _buffer.Length)
            {
                Byte[] newBuffer = new Byte[newCapacity];
                Array.Copy(_buffer, 0, newBuffer, 0, _buffer.Length);
                _buffer = newBuffer;
            }
        }

        internal byte[] Data { get => _buffer; }
    }
}
