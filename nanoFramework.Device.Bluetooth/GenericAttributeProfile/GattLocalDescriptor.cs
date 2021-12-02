//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  This class defines a descriptor of a local characteristic.
    /// </summary>
    public sealed class GattLocalDescriptor
    {
        private static ushort GattLocalDescriptorIndex = 0;

        // Each Descriptor will have unique _descriptorId for event lookup, events for descriptors are handled by User app.
        // This comprises of characteristic ID + GattLocalDescriptorIndex in the form
        // x'DDCC' where DD is Descriptor and CC characteristic
        internal ushort _descriptorId;

        private readonly GattLocalCharacteristic _charactisic;

        private readonly GattProtectionLevel _writeProtectionLevel;
        private readonly GattProtectionLevel _readProtectionLevel;
        private readonly byte[] _uuid;

        private readonly Buffer _staticValue;

        /// <summary>
        /// Delegate for Read requests
        /// </summary>
        /// <param name="sender">GattLocalDescriptor sending event</param>
        /// <param name="ReadRequestEventArgs">Event arguments</param>
        public delegate void GattLocalDescriptorReadEventHandler(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs);

        /// <summary>
        /// Delegate for Write requests
        /// </summary>
        /// <param name="sender">GattLocalDescriptor sending event</param>
        /// <param name="WriteRequestEventArgs">Event arguments</param>
        public delegate void GattLocalDescriptorWriteEventHandler(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs);

        internal GattLocalDescriptor(Guid uuid, GattLocalDescriptorParameters parameters, GattLocalCharacteristic charactisic)
        {
            _uuid = uuid.ToByteArray();
            _charactisic = charactisic;

            _writeProtectionLevel = parameters.WriteProtectionLevel;
            _readProtectionLevel = parameters.ReadProtectionLevel;
            _staticValue = parameters.StaticValue;

            _descriptorId = (ushort)((NextDescriptorIndex() << 8) + _charactisic._characteristicId);
        }

        private static ushort NextDescriptorIndex()
        {
            return ++GattLocalDescriptorIndex;
        }

        /// <summary>
        ///  Gets the read protection level of this local characteristic descriptor.
        /// </summary>
        public GattProtectionLevel ReadProtectionLevel { get => _readProtectionLevel; }

        /// <summary>
        ///  Gets the static value for this local characteristic descriptor.
        /// </summary>
        public Buffer StaticValue { get => _staticValue; }

        /// <summary>
        ///  Gets the Bluetooth SIG-defined UUID for this local characteristic descriptor.
        /// </summary>
        public Guid Uuid { get => new Guid(_uuid); }

        /// <summary>
        ///  Gets the write protection level.
        /// </summary>
        public GattProtectionLevel WriteProtectionLevel { get => _writeProtectionLevel; }

        /// <summary>
        /// An event that is triggered when a GATT client requests a descriptor read operation.
        /// </summary>
        public event GattLocalDescriptorReadEventHandler ReadRequested;

        /// <summary>
        /// This is an event that is triggered when a write descriptor was requested.
        /// </summary>
        public event GattLocalDescriptorWriteEventHandler WriteRequested;

        internal bool OnReadRequested(GattReadRequestedEventArgs e)
        {
            if (_staticValue != null)
            {
                e.GetRequest().RespondWithValue(_staticValue);
                return true;
            }

            if (ReadRequested != null)
            {
                ReadRequested?.Invoke(_charactisic, e);
                return true;
            }

            return false;
        }

        internal bool OnWriteRequested(GattWriteRequestedEventArgs e)
        {
            if (WriteRequested != null)
            {
                WriteRequested?.Invoke(_charactisic, e);
                return true;
            }

            return false;
        }
    }
}