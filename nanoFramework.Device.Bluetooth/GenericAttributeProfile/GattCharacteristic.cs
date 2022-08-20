//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a Characteristic of a GATT service. The GattCharacteristic object
    /// represents a GATT Characteristic of a particular service, and is obtained from
    /// the Characteristics property of the GattDeviceService object.
    /// </summary>
    public class GattCharacteristic : IGattAttribute
    {
        private readonly GattDeviceService _service;
        private readonly ushort _attributeHandle;
        private ushort _lastHandle; // used for Descriptor discovery
        private readonly byte[] _uuid;
        private readonly ArrayList _descriptors;
        private ArrayList _presentationFormats;
        private string _userDescription;
        private readonly GattCharacteristicProperties _characteristicProperties;

        private ushort _cccdDescriptorHandle = 0;

        private GattProtectionLevel _protectionLevel = GattProtectionLevel.Plain;

        private readonly AutoResetEvent _completedEvent = new(false);

        private const int operationTimeout = 7000;

        /// <summary>
        /// Delegate for ValueChanged events.
        /// </summary>
        /// <param name="sender">GattCharacteristic sender object.</param>
        /// <param name="valueChangedEventArgs">ValueChanged event arguments.</param>
        public delegate void GattCharacteristicVlaueChangedEventHandler(GattCharacteristic sender, GattValueChangedEventArgs valueChangedEventArgs);

        internal GattCharacteristic(GattDeviceService service, ushort attributeHandle)
        {
            _service = service;

            _attributeHandle = attributeHandle;

            _characteristicProperties = 0;

            // Create a buffer to be written to by Native code.
            _uuid = new byte[16];

            _descriptors = new();

            // Null is uninitialised flag for properties
            _userDescription = null;
            _presentationFormats = null;
        }

        // Used for descriptor discovery
        internal ushort LastHandle { get => _lastHandle; set => _lastHandle = value; }

        /// <summary>
        /// Performs a Characteristic Value read from device.
        /// </summary>
        /// <returns></returns>
        public GattReadResult ReadValue()
        {
            // Make sure its readable otherwise give error
            if (CharacteristicProperties.HasFlag(GattCharacteristicProperties.Read))
            {
                return _service.Device.ReadAttributeValue(AttributeHandle);
            }

            return new GattReadResult(null, GattCommunicationStatus.AccessDenied, 0);
        }

        /// <summary>
        ///  Performs a Characteristic Value write to a Bluetooth LE device.
        /// </summary>
        /// <param name="value">
        /// A Buffer object which contains the data to be written to the Bluetooth LE device.
        /// </param>
        /// <param name="writeOption">Specifies what type of GATT write should be performed.</param>
        /// <returns>
        /// An operation completes with a GattWriteResult object.
        /// </returns>
        public GattWriteResult WriteValueWithResult(Buffer value, GattWriteOption writeOption = GattWriteOption.WriteWithoutResponse)
        {
            bool writeable = CharacteristicProperties.HasFlag(GattCharacteristicProperties.Write);
            bool writeNoResponse = CharacteristicProperties.HasFlag(GattCharacteristicProperties.WriteWithoutResponse);

            // Make sure its writeable otherwise give error
            if ((writeable && writeOption == GattWriteOption.WriteWithResponse) ||
                (writeNoResponse && writeOption == GattWriteOption.WriteWithoutResponse))
            {
                return _service.Device.WriteAttributeValueWithResult(_attributeHandle, value, writeOption);
            }

            return new GattWriteResult(GattCommunicationStatus.AccessDenied, 0);
        }


        /// <summary>
        ///  Reads the current value of the ClientCharacteristicConfigurationDescriptor to enable notify / Indicate.
        /// </summary>
        /// <returns>
        /// Upon completion of the method, the GattReadClientCharacteristicConfigurationDescriptorResult contains
        /// the result of the read operation, which contains the status of completed operation. 
        /// The Status property on the GattReadClientCharacteristicConfigurationDescriptorResult returned 
        /// indicates if the result of the operation was successful.
        /// </returns>
        public GattReadClientCharacteristicConfigurationDescriptorResult ReadClientCharacteristicConfigurationDescriptor()
        {
            GattCommunicationStatus status = GattCommunicationStatus.Unreachable;
            GattClientCharacteristicConfigurationDescriptorValue dv = GattClientCharacteristicConfigurationDescriptorValue.None;
            byte protcolError = 0;

            GetDescriptorsIfRequired();

            if (_cccdDescriptorHandle != 0)
            {
                GattReadResult grr = _service.Device.ReadAttributeValue(_cccdDescriptorHandle);
                if (grr.Status == 0)
                {
                    DataReader rdr = DataReader.FromBuffer(grr.Value);
                    byte val = rdr.ReadByte();

                    dv = (GattClientCharacteristicConfigurationDescriptorValue)val;
                    status = GattCommunicationStatus.Success;
                }
                else
                {
                    protcolError = grr.ProtocolError;
                }
            }

            return new GattReadClientCharacteristicConfigurationDescriptorResult(
                        dv,
                        status,
                        protcolError);
        }

        /// <summary>
        /// Writes the ClientCharacteristicConfigurationDescriptor to the Bluetooth LE device,
        /// and if the value to be written represents an indication or a notification and
        /// a ValueChanged event handler is registered, enables receiving ValueChanged events
        /// from the device.
        /// </summary>
        /// <param name="clientCharacteristicConfigurationDescriptorValue">
        /// Specifies a new value for the ClientCharacteristicConfigurationDescriptor of this Characteristic object.
        /// </param>
        /// <returns>
        /// Returns an asynchronous operation that completes with a GattWriteResult object.
        /// </returns>
        public GattWriteResult WriteClientCharacteristicConfigurationDescriptorWithResult(GattClientCharacteristicConfigurationDescriptorValue clientCharacteristicConfigurationDescriptorValue)
        {
            GattCommunicationStatus status = GattCommunicationStatus.AccessDenied;
            byte protocolError = 0;

            GetDescriptorsIfRequired();

            // Does Characteristic have a CCCD ?
            if (_cccdDescriptorHandle != 0)
            {
                if (_characteristicProperties.HasFlag(GattCharacteristicProperties.Notify) &&
                clientCharacteristicConfigurationDescriptorValue == GattClientCharacteristicConfigurationDescriptorValue.Notify ||
                _characteristicProperties.HasFlag(GattCharacteristicProperties.Indicate) &&
                clientCharacteristicConfigurationDescriptorValue == GattClientCharacteristicConfigurationDescriptorValue.Indicate ||
                clientCharacteristicConfigurationDescriptorValue == GattClientCharacteristicConfigurationDescriptorValue.None)
                {
                    // CCCD is 2 bytes, first byte is none/notify/indicate value
                    DataWriter dw = new();
                    dw.WriteByte((byte)clientCharacteristicConfigurationDescriptorValue);
                    dw.WriteByte(0);

                    GattWriteResult wr = _service.Device.WriteAttributeValueWithResult(
                        _cccdDescriptorHandle,
                        dw.DetachBuffer(),
                        GattWriteOption.WriteWithoutResponse);

                    if (wr.Status == 0)
                    {
                        status = GattCommunicationStatus.Success;
                    }
                    else
                    {
                        protocolError = wr.ProtocolError;
                        status = GattCommunicationStatus.Unreachable;
                    }
                }
            }
            return new(status, protocolError);
        }

        private void GetDescriptorsIfRequired()
        {
            // No descriptors and 
            if (_descriptors.Count == 0)
            {
                // Try to read them
                GetDescriptors();
            }
        }
        /// <summary>
        /// Returns the descriptors for this GattCharacteristic instance.
        /// </summary>
        /// <returns>GattDescriptorsResult</returns>
        public GattDescriptorsResult GetDescriptors()
        {
            ushort result;
            GattCommunicationStatus status = GattCommunicationStatus.Unreachable;

            // Only allow 1 simultaneous call per device connection
            lock (_service.Device.lockObj)
            {
                if (_descriptors.Count == 0)
                {
                    ushort lastHandle = _service.GetLastHandleForCharacteristic(this);
                    result = NativeStartDiscoveryDescriptors(_service.Device.ConnectionHandle, _attributeHandle, lastHandle);
                    if (result == 0)
                    {
                        if (_completedEvent.WaitOne(operationTimeout, false))
                        {
                            status = GattCommunicationStatus.Success;
                        }
                    }
                }
                else
                {
                    status = GattCommunicationStatus.Success;
                }

                return new(0, _descriptors, status);
            }
        }

        /// <summary>
        /// Returns the descriptors whose UUIDs match descriptorUuid.
        /// </summary>
        /// <param name="descriptorUuid">The UUID for the descriptors to be retrieved.</param>
        /// <returns>
        /// Completes with the GattDescriptorsResult which contain the descriptors whose UUIDs match descriptorUuid.
        /// </returns>
        public GattDescriptorsResult GetDescriptorsForUuid(Guid descriptorUuid)
        {
            ArrayList selectedDes = new();
            byte protocolError = 0;
            GattCommunicationStatus status = GattCommunicationStatus.Success;

            // No descriptor yet then try to load them
            if (_descriptors.Count == 0)
            {
                GattDescriptorsResult res = GetDescriptors();

                status = res.Status;
                protocolError = res.ProtocolError;
            }

            foreach (GattDescriptor ds in _descriptors)
            {
                if (descriptorUuid.Equals(ds.Uuid))
                {
                    selectedDes.Add(ds);
                }
            }

            return new GattDescriptorsResult(protocolError, selectedDes, status);
        }

        /// <summary>
        ///  Gets or sets the desired GATT security options for over the air communication
        ///  with the device.
        /// </summary>
        public GattProtectionLevel ProtectionLevel
        {
            get => _protectionLevel;
            set
            {
                // TODO - add security
                _protectionLevel = value;
            }
        }

        /// <summary>
        /// Gets the handle used to uniquely identify GATT-based characteristic attributes
        /// as declared on the Bluetooth LE device.
        /// </summary>
        public ushort AttributeHandle { get => _attributeHandle; }

        /// <summary>
        /// Gets the GATT characteristic properties, as defined by the GATT profile.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties { get => _characteristicProperties; }

        /// <summary>
        /// Gets an array of presentation format descriptors associated with this GattCharacteristic,
        /// in the order specified by the Aggregate Format Descriptor.
        /// </summary>
        public GattPresentationFormat[] PresentationFormats
        {
            get
            {
                // Initialize from _descriptors first time called
                if (_presentationFormats == null)
                {
                    GetDescriptorsIfRequired();

                    _presentationFormats = new();

                    // Parse descriptors
                    foreach (GattDescriptor gd in _descriptors)
                    {
                        if (gd.Uuid.Equals(GattDescriptorUuids.CharacteristicPresentationFormat))
                        {
                            // Presentation format , update property arraylist
                            GattReadResult rr = gd.ReadValue();
                            if (rr.Status == 0)
                            {
                                DataReader rdr = DataReader.FromBuffer(rr.Value);

                                byte formatType = rdr.ReadByte();
                                int exponent = (int)rdr.ReadByte();  // should be a signed byte
                                ushort unit = rdr.ReadUInt16();
                                byte nameSpaceId = rdr.ReadByte();
                                ushort description = rdr.ReadUInt16();

                                // Presentation formats, update property
                                _presentationFormats.Add(new GattPresentationFormat(formatType, exponent, unit, nameSpaceId, description));
                            }
                        }
                    }
                }

                return (GattPresentationFormat[])_presentationFormats.ToArray(typeof(GattPresentationFormat));
            }
        }

        /// <summary>
        /// Get the user friendly description for this GattCharacteristic, if the User Description
        /// Descriptor is present, otherwise this will be an empty string.
        /// </summary>
        public string UserDescription
        {
            get
            {
                // First time called pull user description from descriptor if any
                if (_userDescription == null)
                {
                    GetDescriptorsIfRequired();

                    // Parse descriptors
                    foreach (GattDescriptor gd in _descriptors)
                    {
                        if (gd.Uuid.Equals(GattDescriptorUuids.CharacteristicUserDescription))
                        {
                            // User description , update property
                            GattReadResult rr = gd.ReadValue();
                            if (rr.Status == 0)
                            {
                                DataReader rdr = DataReader.FromBuffer(rr.Value);
                                byte[] stringBytes = new byte[rdr.UnconsumedBufferLength];
                                rdr.ReadBytes(stringBytes);

                                _userDescription = Encoding.UTF8.GetString(stringBytes, 0, stringBytes.Length);
                                break;
                            }
                        }
                    }
                }

                if (_userDescription == null)
                {
                    // If no user description descriptor then set as "" 
                    _userDescription = "";
                }

                return _userDescription;
            }
        }

        /// <summary>
        ///  Gets the GATT Characteristic UUID for this GattCharacteristic.
        /// </summary>
        public Guid Uuid { get => new(_uuid); }

        /// <summary>
        ///  Gets the GattDeviceService of which this characteristic is a member.
        /// </summary>
        public GattDeviceService Service { get => _service; }

        /// <summary>
        /// An Application can register an event handler in order to receive events when notification
        /// or indications are received from a device.
        /// You will need to set the Client Characteristic Configuration Descriptor before subscribing to event.
        /// </summary>
        public event GattCharacteristicVlaueChangedEventHandler ValueChanged;

        /// <summary>
        /// Route events from Native code.
        /// </summary>
        /// <param name="btEvent"></param>
        internal void OnEvent(BluetoothEventCentral btEvent)
        {
            switch (btEvent.type)
            {
                case BluetoothEventType.AttributeValueChanged:

                    // Get value for event
                    byte[] data = NativeReadEventData(btEvent.connectionHandle, _attributeHandle);
                    Buffer buffer = new(data);

                    ValueChanged?.Invoke(this, new GattValueChangedEventArgs(buffer, DateTime.UtcNow));
                    break;

                case BluetoothEventType.DescriptorDiscovered:
                    {
                        //Debug.WriteLine($"# BluetoothEventType.DescriptorDiscovered status:{btEvent.status} attH:{_attributeHandle} chrH:{btEvent.characteristicHandle}");

                        GattDescriptor gd = new(this, 0);
                        NativeUpdateDescriptor(btEvent.connectionHandle, _service.AttributeHandle, _attributeHandle, gd);

                        _descriptors.Add(gd);
                        _service.Device.AttributeAdd(gd);

                        if (gd.Uuid.Equals(GattDescriptorUuids.ClientCharacteristicConfiguration))
                        {
                            // Save reference to CCCD descriptor for methods
                            _cccdDescriptorHandle = gd.AttributeHandle;
                        }
                    }
                    break;

                case BluetoothEventType.DescriptorDiscoveryComplete:
                    {
                        //Debug.WriteLine($"# BluetoothEventType.DescriptorDiscoveryComplete status:{btEvent.status} attH:{_attributeHandle} chrH:{btEvent.characteristicHandle}");

                        //Debug.WriteLine($"# DescriptorDiscoveryComplete _completedEvent.Set");
                        _completedEvent.Set();
                    }
                    break;
            }
        }

        #region Native

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeStartDiscoveryDescriptors(ushort connectHandle, ushort val_Handle, ushort last_Handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeUpdateDescriptor(ushort connectHandle, ushort serviceHandle, ushort valueHandle, GattDescriptor gd);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern byte[] NativeReadEventData(ushort connectHandle, ushort valueHandle);

        #endregion

    }
}
