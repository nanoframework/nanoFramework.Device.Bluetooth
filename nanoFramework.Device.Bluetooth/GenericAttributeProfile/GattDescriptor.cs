//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Descriptor of a GATT Characteristic. The GattDescriptor object represents
    /// a GATT Descriptor of a particular characteristic, it is obtained from the Descriptors
    /// property of the GattCharacteristic object.
    /// </summary>
    public class GattDescriptor : IGattAttribute
    {
        private readonly ushort _attributeHandle;
        private readonly byte[] _uuid;
        private GattProtectionLevel _protectionLevel;
        private readonly BluetoothLEDevice _device;

        internal GattDescriptor(GattCharacteristic characteristic, ushort attributeHandle)
        {
            _device = characteristic.Service.Device;
            _attributeHandle = attributeHandle;
            _uuid = new byte[16];
        }

        /// <summary>
        /// Performs a Descriptor Value read.
        /// </summary>
        /// <returns>
        /// The object required to manage the asynchronous operation, which, upon completion,
        /// returns a GattReadResult object, which in turn contains the completion status
        /// of the asynchronous operation and, if successful, the data read from the device.
        /// </returns>
        public GattReadResult ReadValue()
        {
            return _device.ReadAttributeValue(AttributeHandle);
        }

        /// <summary>
        /// Performs a Descriptor Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">
        /// A Buffer object which contains the data to be written
        /// to the Bluetooth LE device.
        /// </param>
        /// <returns>
        /// Returns the status with which the operation completed.
        /// </returns>
        public GattCommunicationStatus WriteValue(Buffer value)
        {
            GattWriteResult gwr = _device.WriteAttributeValueWithResult(AttributeHandle, value);

            return gwr.Status;
        }

        /// <summary>
        /// Performs a Descriptor Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">
        /// A Buffer object which contains the data to be written
        /// to the Bluetooth LE device.
        /// </param>
        /// <returns>
        /// A GattWriteResult.
        /// </returns>
        public GattWriteResult WriteValueWithResult(Buffer value)
        {
            return _device.WriteAttributeValueWithResult(AttributeHandle, value);
        }

        /// <summary>
        /// Gets or sets the desired GATT security options for over the air communication
        /// with the device.
        /// </summary>
        public GattProtectionLevel ProtectionLevel { get => _protectionLevel; set => _protectionLevel = value; }

        /// <summary>
        /// Gets the GATT Attribute handle used to uniquely identify this attribute on the
        /// GATT Server Device.
        /// </summary>
        public ushort AttributeHandle { get => _attributeHandle; }

        /// <summary>
        /// Gets the GATT Descriptor UUID for this GattDescriptor.
        /// </summary>
        public Guid Uuid { get => new(_uuid); }
    }
}
