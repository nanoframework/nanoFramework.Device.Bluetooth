//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  This class represents a local characteristic.
    /// </summary>
    public class GattLocalCharacteristic
    {
        private static ushort GattLocalCharacteristicIndex = 0;

        // Each Characteristic will have unique _CharacteristicId for event lookup
        internal ushort _characteristicId;

        private ushort _descriptorNextID;
        private readonly byte[] _characteristicUuid;
        private readonly GattProtectionLevel _writeProtectionLevel;
        private readonly GattProtectionLevel _readProtectionLevel;
        private readonly GattCharacteristicProperties _properties;
        private readonly ArrayList _descriptors;
        private readonly ArrayList _subscribedClients;

        // Built in descriptors
        private readonly string _userDescription;
        private readonly GattLocalDescriptor _userDescriptionDescriptor;

        private readonly ArrayList _presentationFormats;
        private readonly ArrayList _presentationFormatsDescriptors;  // of GattLocalDescriptor

        private readonly Buffer _staticValue;

        /// <summary>
        /// Delegate for Read requests
        /// </summary>
        /// <param name="sender">GattLocalCharacteristic sending event</param>
        /// <param name="ReadRequestEventArgs">Event arguments</param>
        public delegate void GattLocalCharacteristicReadEventHandler(GattLocalCharacteristic sender, GattReadRequestedEventArgs ReadRequestEventArgs);

        /// <summary>
        /// Delegate for Write requests
        /// </summary>
        /// <param name="sender">GattLocalCharacteristic sending event</param>
        /// <param name="WriteRequestEventArgs">Event arguments</param>
        public delegate void GattLocalCharacteristicWriteEventHandler(GattLocalCharacteristic sender, GattWriteRequestedEventArgs WriteRequestEventArgs);

        /// <summary>
        /// Delegate for Clients Changed events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public delegate void GattLocalCharacteristicClientsChangedEventHandler(GattLocalCharacteristic sender, object args);

        internal GattLocalCharacteristic(Guid characteristicUuid, GattLocalCharacteristicParameters parameters)
        {
            _characteristicUuid = characteristicUuid.ToByteArray();
            _writeProtectionLevel = parameters.WriteProtectionLevel;
            _readProtectionLevel = parameters.ReadProtectionLevel;

            _properties = parameters.CharacteristicProperties;

            _staticValue = parameters.StaticValue;

            _descriptors = new ArrayList();
            _subscribedClients = new ArrayList();

            // Give it next id
            _characteristicId = NextCharacteristicIndex();
            // Start at 1 for descriptors
            _descriptorNextID = 1;

            _userDescription = parameters.UserDescription;
            if (!string.IsNullOrEmpty(_userDescription))
            {
                GattLocalDescriptorParameters dp = new();

                // Create Static value for User description
                DataWriter dr = new();
                dr.WriteString(_userDescription);
                dp.StaticValue = dr.DetachBuffer();

                _userDescriptionDescriptor = new GattLocalDescriptor(GattDescriptorUuids.CharacteristicUserDescription, dp, this, _descriptorNextID++);
            }

            _presentationFormats = new ArrayList();
            _presentationFormatsDescriptors = new ArrayList();
            foreach (GattPresentationFormat gpf in parameters.PresentationFormats)
            {
                _presentationFormats.Add(gpf);

                // Create Static value for PresentationFormat
                DataWriter dr = new();

                dr.WriteByte(gpf.FormatType);
                
                // Create signed byte from int Exponent
                sbyte exp = (sbyte)gpf.Exponent;
                dr.WriteByte((byte)exp);

                dr.WriteUInt16(gpf.Unit);
                dr.WriteByte(gpf.Namespace);
                dr.WriteUInt16(gpf.Description);

                GattLocalDescriptorParameters dp = new();
                dp.StaticValue = dr.DetachBuffer();

                _presentationFormatsDescriptors.Add(new GattLocalDescriptor(GattDescriptorUuids.CharacteristicPresentationFormat, dp, this, _descriptorNextID++));
            }

            // Register with Events
            BluetoothLEServer._bluetoothEventManager.AddCharacteristic(this);
        }

        private static ushort NextCharacteristicIndex()
        {
            return ++GattLocalCharacteristicIndex;
        }

        /// <summary>
        /// Creates descriptor for this local characteristic.
        /// </summary>
        /// <param name="descriptorUuid">The descriptor UUID.</param>
        /// <param name="parameters">The parameters for the descriptor.</param>
        /// <returns>A GattLocalDescriptorResult object.</returns>
        public GattLocalDescriptorResult CreateDescriptor(Guid descriptorUuid, GattLocalDescriptorParameters parameters)
        {
            GattLocalDescriptor decriptor = null;
            BluetoothError result = BluetoothError.Success;

            // Validate uuid
            // Not already present in array 
            foreach (GattLocalDescriptor des in _descriptors)
            {
                if (descriptorUuid.Equals(des.Uuid))
                {
                    result = BluetoothError.ResourceInUse;
                    break;
                }
            }

            // Not a standard descriptor
            ushort suuid = Utilities.ConvertUuidToShortId(descriptorUuid);
            if (suuid == (ushort)Utilities.GattNativeDescriptorUuid.CharacteristicUserDescription ||
                 suuid == (ushort)Utilities.GattNativeDescriptorUuid.CharacteristicPresentationFormat)
            {
                result = BluetoothError.ResourceInUse;
            }

            if (result == BluetoothError.Success)
            {
                decriptor = new GattLocalDescriptor(descriptorUuid, parameters, this, _descriptorNextID++);
                _descriptors.Add(decriptor);
            }

            return new GattLocalDescriptorResult(decriptor, result);
        }

        /// <summary>
        /// Send and notifies all subscribed clients a GattSubscribedClient of a value.
        /// </summary>
        /// <param name="value">The buffer that contains the value to send to the GattSubscribedClient.</param>
        /// <returns>
        /// An array of all the GattClientNotificationResult for each subscribed client.
        /// </returns>
        public GattClientNotificationResult[] NotifyValue(Buffer value)
        {
            GattClientNotificationResult[] results = new GattClientNotificationResult[_subscribedClients.Count];

            lock (_subscribedClients)
            {
                for (int index = 0; index < _subscribedClients.Count; index++)
                {
                    GattSubscribedClient client = (GattSubscribedClient)_subscribedClients[index];
                    results[index] = NotifyValue(value, client);
                }
            }

            return results;
        }

        /// <summary>
        /// Sends and notifies a GattSubscribedClient of a value.
        /// </summary>
        /// <param name="value">The buffer that contains the value to send to the GattSubscribedClient</param>
        /// <param name="subscribedClient">The subscribed client notify the value.</param>
        /// <returns></returns>
        public GattClientNotificationResult NotifyValue(Buffer value, GattSubscribedClient subscribedClient)
        {
            GattClientNotificationResult result;

            Byte[] buffer = new byte[value.Length];
            Array.Copy(value.Data, buffer, (int)value.Length);

            int rc = NativeNotifyClient((ushort)subscribedClient.Session.DeviceId.Id, _characteristicId, buffer);

            result = new GattClientNotificationResult((byte)rc, rc == 0 ? GattCommunicationStatus.Success : GattCommunicationStatus.Unreachable, subscribedClient, (ushort)value.Length);

            return result;
        }

        /// <summary>
        /// Gets the local characteristic properties.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties { get => _properties; }

        /// <summary>
        ///  Gets a vector list of all the descriptors for this local characteristic.
        /// </summary>
        public GattLocalDescriptor[] Descriptors { get => (GattLocalDescriptor[])_descriptors.ToArray(typeof(GattLocalDescriptor)); }

        /// <summary>
        /// Gets the presentation formats for this local characteristic.
        /// </summary>
        public GattPresentationFormat[] PresentationFormats { get => (GattPresentationFormat[])_presentationFormats.ToArray(typeof(GattPresentationFormat)); }

        /// <summary>
        /// Gets the read protection level of this local characteristic.
        /// </summary>
        public GattProtectionLevel ReadProtectionLevel { get => _readProtectionLevel; }

        /// <summary>
        /// Gets the static value for this local GATT characteristic.
        /// </summary>
        public Buffer StaticValue { get => _staticValue; }

        /// <summary>
        /// Gets an array of all clients that are subscribed to this local characteristic.
        /// </summary>
        public GattSubscribedClient[] SubscribedClients { get => (GattSubscribedClient[])_subscribedClients.ToArray(typeof(GattSubscribedClient)); }

        /// <summary>
        /// Gets the user-friendly description for this local characteristic.
        /// </summary>
        public string UserDescription { get => _userDescription; }

        /// <summary>
        /// Gets the BluetoothSIG-defined UUID for this local characteristic.
        /// </summary>
        public Guid Uuid { get => new(_characteristicUuid); }

        /// <summary>
        /// Gets the write protection level of this local characteristic.
        /// </summary>
        public GattProtectionLevel WriteProtectionLevel { get => _writeProtectionLevel; }

        /// <summary>
        /// An event that is triggered when a GATT client requests a read operation.
        /// </summary>
        public event GattLocalCharacteristicReadEventHandler ReadRequested;

        /// <summary>
        /// This is an event that is triggered when a write was requested.
        /// </summary>
        public event GattLocalCharacteristicWriteEventHandler WriteRequested;

        /// <summary>
        ///  An event that triggers when clients subscribed to this local characteristic changes.
        /// </summary>
        public event GattLocalCharacteristicClientsChangedEventHandler SubscribedClientsChanged;

        internal void OnReadRequested(ushort descritorId, GattReadRequestedEventArgs e)
        {
            bool handled = false;

            // Static value for Characteristic ?
            int descritorIndex = (descritorId >> 8);
            if (_staticValue != null && descritorIndex == 0)
            {
                handled = true;
                // Handle static values internally, don't fire an event
                DataWriter writer = new();
                writer.WriteBuffer(_staticValue);
                e.GetRequest().RespondWithValue(_staticValue);
            }
            else if (descritorIndex != 0)
            {
                // Descriptor event, let descriptor handle it
                GattLocalDescriptor des = FindDescriptor(descritorId);
                if (des != null)
                {
                    handled = des.OnReadRequested(e);
                }
            }
            else if (ReadRequested != null)
            {
                // LocalCharacteristic event ?
                handled = true;
                ReadRequested?.Invoke(this, e);
            }

            if (!handled)
            {
                // No event handler, respond with error
                e.GetRequest().RespondWithProtocolError(GattProtocolError.UnlikelyError);
            }
        }

        internal void OnWriteRequested(ushort descritorId, GattWriteRequestedEventArgs e)
        {
            bool handled = false;

            if (WriteRequested != null)
            {
                int descritorIndex = (descritorId >> 8);  

                // LocalCharacteristic event ?
                if (descritorIndex == 0)
                {
                    handled = true;
                    WriteRequested?.Invoke(this, e);
                }
                else
                {
                    // Descriptor event
                    GattLocalDescriptor des = FindDescriptor(descritorId);
                    if (des != null)
                    {
                        handled = des.OnWriteRequested(e);
                    }
                }
            }

            if (!handled)
            {
                // No event handler, respond with error
                e.GetRequest().RespondWithProtocolError(GattProtocolError.UnlikelyError);
            }
        }

        private int ClientSubscribed(GattSubscribedClient client)
        {
            lock (_subscribedClients)
            {
                for (int index = 0; index < _subscribedClients.Count; index++)
                {
                    if (((GattSubscribedClient)_subscribedClients[index]).Session.DeviceId.Id == client.Session.DeviceId.Id)
                    {
                        return index;
                    }
                }
            }

            return -1;
        }

        internal void OnSubscribedClientsChanged(bool subscribe, GattSubscribedClient client)
        {
            lock (_subscribedClients)
            {
                int index = ClientSubscribed(client);

                if (subscribe)
                {
                    if (index < 0)
                    {
                        _subscribedClients.Add(client);
                    }
                }
                else
                {
                    if (index >= 0)
                    {
                        _subscribedClients.RemoveAt(index);
                    }
                }
            }

            SubscribedClientsChanged?.Invoke(this, null);
        }

        private GattLocalDescriptor FindDescriptor(ushort id)
        {
            // Check In built ones first
            if (_userDescriptionDescriptor != null && _userDescriptionDescriptor._descriptorId == id)
            {
                return _userDescriptionDescriptor;
            }

            // Check PresentationFormats
            foreach (GattLocalDescriptor desc in _presentationFormatsDescriptors)
            {
                if (desc._descriptorId == id)
                {
                    return desc;
                }
            }

            // Check other descriptors
            foreach (GattLocalDescriptor desc in _descriptors)
            {
                if (desc._descriptorId == id)
                {
                    return desc;
                }
            }
            return null;
        }

        #region Native
        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern int NativeNotifyClient(ushort connection, ushort CharacteristicId, byte[] notifyBuffer);

        #endregion
    }
}