﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device.
    /// </summary>
    public class BluetoothLEDevice : IDisposable
    {
        private BluetoothConnectionStatus _connectionStatus;
        private readonly ulong _bluetoothAddress;
        private readonly BluetoothAddressType _addressType;
        private string _name;
        private ushort _appearance;
        private ushort _connectionHandle;
        private bool _genericAccessRead;

        private ArrayList _services;
        private Hashtable _attributes;

        private readonly DevicePairing _pairing;

        private bool _disposed;
        private readonly AutoResetEvent _eventComplete;
        private readonly AutoResetEvent _completedEvent = new(false);
        internal readonly object lockObj = new();

        private byte[] _eventValue;
        private ushort _eventStatus;

        private const int operationTimeout = 7000;

        private static ushort _routingHandle = 0xff00;

        enum readWriteValueResult { success, acessDenied, failure };

        internal BluetoothLEDevice(ulong bluetoothAddress, BluetoothAddressType addressType)
        {
            _bluetoothAddress = bluetoothAddress;
            _addressType = addressType;

            _name = "";
            _appearance = 0;    // Unknown
            _genericAccessRead = false;

            _connectionStatus = BluetoothConnectionStatus.Disconnected;

            _services = new();
            _attributes = new();
            _pairing = new(this);

            _eventComplete = new AutoResetEvent(false);
        }

        /// <summary>
        /// We use a dummy connection handle for routing events while connecting
        /// which is then updated when we connect successfully with correct connection handle.
        /// </summary>
        private ushort GetConnectingHandle
        {
            get
            {
                _routingHandle--;
                _routingHandle = ((int)_routingHandle < 0xff00) ? (ushort)0xfffe : _routingHandle;
                return _routingHandle;
            }
        }

        internal ushort ConnectionHandle { get => _connectionHandle; set => _connectionHandle = value; }

        internal GattCommunicationStatus ConnectDeviceIfNotConnected()
        {
            // Check and set mode, throw exception if wrong state
            BluetoothNanoDevice.CheckMode(BluetoothNanoDevice.Mode.Client);

            GattCommunicationStatus status = GattCommunicationStatus.Success;

            if (_connectionStatus == BluetoothConnectionStatus.Disconnected)
            {
                // Give it a dummy connection handle until we have a real one
                ConnectionHandle = GetConnectingHandle;

                // Add device to event listener so event get routed here
                BluetoothLEServer._bluetoothEventManager.AddLeDevice(this);

                //Debug.WriteLine($"# ConnectDeviceIfNot ConnectionHandle {ConnectionHandle}");

                // Start a connect to peer device
                ushort connectStatus = NativeConnect(_bluetoothAddress, _addressType, ConnectionHandle);
                //Debug.WriteLine($"# NativeConnect status {connectStatus}");
                if (connectStatus == 0)
                {
                    // Wait for connect to complete via event
                    // Event will set _connectionStatus
                    if (_eventComplete.WaitOne(10000, false))
                    {
                        if (_connectionStatus == BluetoothConnectionStatus.Connected)
                        {
                            status = GattCommunicationStatus.Success;
                            //Debug.WriteLine($"# NativeConnect connected");
                        }
                    }
                    else
                    {
                        //Debug.WriteLine($"# NativeConnect timeout");

                    }
                }

                if (status != GattCommunicationStatus.Success)
                {
                    // Error so remove from listener
                    BluetoothLEServer._bluetoothEventManager.RemoveLeDevice(this);
                    status = GattCommunicationStatus.Unreachable;
                }
            }

            return status;
        }

        private void DisconnectDevice()
        {
            if (_connectionStatus == BluetoothConnectionStatus.Connected)
            {
                NativeDisconnect(_connectionHandle);
            }
        }

        /// <summary>
        /// Closes the connection to the Bluetooth LE device.
        /// </summary>
        public void Close()
        {
            DisconnectDevice();
        }

        /// <summary>
        /// Gets the GattDeviceServices for this Bluetooth LowEnergy device.
        /// </summary>
        public GattDeviceServicesResult GetGattServices()
        {
            GattCommunicationStatus status = ConnectDeviceIfNotConnected();

            // we already have services then we must be trying to re-connect
            // Just return current services
            if (status == GattCommunicationStatus.Success &&
                _services.Count == 0)
            {
                try
                {
                    // Start discovery of Gatt Services on Device and wait for events
                    //Debug.WriteLine("# Start NativeDiscoverServices");
                    ushort error = NativeDiscoverServices(_connectionHandle);
                    //Debug.WriteLine($"# Exit NativeDiscoverServices {error}");
                    if (error == 0)
                    {
                        if (_eventComplete.WaitOne(20000, true))
                        {
                            //Debug.WriteLine($"# wait GetGattServices complete");

                            status = GattCommunicationStatus.Success;

                            // Fire services changed event
                            GattServicesChanged?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            //Debug.WriteLine($"# wait GetGattServices timeout");

                        }
                    }
                    else
                    {
                        status = GattCommunicationStatus.Unreachable;
                    }
                }
                catch (ApplicationException)
                {
                    status = GattCommunicationStatus.AccessDenied;
                }
                catch (TimeoutException)
                {
                    status = GattCommunicationStatus.Unreachable;
                }
            }

            return new GattDeviceServicesResult(0, _services, status);
        }

        /// <summary>
        /// Returns the GattDeviceServices for the Bluetooth LowEnergy device with the specified UUID.
        /// </summary>
        /// <param name="serviceUuid">The service UUID.</param>
        /// <returns>GattDeviceServicesResult</returns>
        public GattDeviceServicesResult GetGattServicesForUuid(Guid serviceUuid)
        {
            ArrayList selectedServices = new();
            GattDeviceServicesResult result;

            // If no services cached then try get services from device first
            if (_services.Count == 0 || _connectionStatus != BluetoothConnectionStatus.Connected)
            {
                GetGattServices();
            }

            // Select services from cache with UUID 
            foreach (object obj in _services)
            {
                GattDeviceService service = obj as GattDeviceService;
                if (service.Uuid.Equals(serviceUuid))
                {
                    selectedServices.Add(service);
                }
            }

            result = new GattDeviceServicesResult(0, selectedServices, GattCommunicationStatus.Success);

            return result;
        }

        /// <summary>
        /// Returns a BluetoothLEDevice object representing the peer device with the given
        /// address and address type. See **Remarks**.
        /// </summary>
        /// <param name="bluetoothAddress">
        /// A BluetoothAddress value containing the 64-bit address of the peer Bluetooth
        /// LE device.
        /// </param>
        /// <param name="bluetoothAddressType">
        /// A BluetoothAddressType value containing the address type of the peer Bluetooth
        /// LE device.
        /// </param>
        /// <returns>
        /// Returns a BluetoothLEDevice object representing the peer device with the given
        /// address and address type.
        /// </returns>
        public static BluetoothLEDevice FromBluetoothAddress(ulong bluetoothAddress, BluetoothAddressType bluetoothAddressType)
        {
            return new BluetoothLEDevice(bluetoothAddress, bluetoothAddressType);
        }

        /// <summary>
        /// Returns a BluetoothLEDevice object representing the peer Bluetooth LE device
        /// with the given address. 
        /// </summary>
        /// <param name="bluetoothAddress">
        /// A BluetoothAddress value containing the 64-bit address of the peer Bluetooth
        /// LE device.
        /// </param>
        /// <returns>
        /// BluetoothLEDevice object representing the peer Bluetooth LE device
        /// with the given address.
        /// </returns>
        public static BluetoothLEDevice FromBluetoothAddress(ulong bluetoothAddress)
        {
            return FromBluetoothAddress(bluetoothAddress, BluetoothAddressType.Public);
        }

        /// <summary>
        /// Gets the device address.
        /// </summary>
        public ulong BluetoothAddress { get => _bluetoothAddress; }

        /// <summary>
        /// Gets the connection status of the device.
        /// </summary>
        public BluetoothConnectionStatus ConnectionStatus
        {
            get => _connectionStatus;
            internal set
            {
                BluetoothConnectionStatus currentStatus = _connectionStatus;
                _connectionStatus = value;

                // Fire event on change
                ConnectionStatusChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the name of the Bluetooth LE device.
        /// </summary>
        public string Name
        {
            get
            {
                LoadGenericAccessValues();
                return _name;
            }
        }

        /// <summary>
        /// Gets the appearance of the Bluetooth LE device.
        /// </summary>
        public ushort Appearance
        {
            get
            {
                LoadGenericAccessValues();
                return _appearance;
            }
        }

        /// <summary>
        /// Access the Generic Access service and retrieve Device name and Appearance to set properties.
        /// </summary>
        private void LoadGenericAccessValues()
        {
            if (!_genericAccessRead && ConnectDeviceIfNotConnected() == GattCommunicationStatus.Success)
            {
                // Get Generic Access Service
                GattDeviceServicesResult sr = GetGattServicesForUuid(GattServiceUuids.GenericAccess);
                if (sr.Status == GattCommunicationStatus.Success)
                {
                    GattDeviceService srv = sr.Services[0];
                    GattCharacteristicsResult cr = srv.GetCharacteristicsForUuid(GattCharacteristicUuids.GapDeviceName);
                    if (cr.Status == GattCommunicationStatus.Success)
                    {
                        // Get device name value
                        GattReadResult grn = ReadAttributeValue(cr.Characteristics[0].AttributeHandle);
                        if (grn.Status == 0)
                        {
                            DataReader rdr = DataReader.FromBuffer(grn.Value);
                            _name = rdr.ReadString(rdr.UnconsumedBufferLength);
                        }
                    }

                    cr = srv.GetCharacteristicsForUuid(GattCharacteristicUuids.GapAppearance);
                    if (cr.Status == GattCommunicationStatus.Success)
                    {
                        // Get device appearance value
                        GattReadResult gra = ReadAttributeValue(cr.Characteristics[0].AttributeHandle);
                        if (gra.Status == 0)
                        {
                            DataReader rdr = DataReader.FromBuffer(gra.Value);
                            _appearance = rdr.ReadUInt16();
                        }
                    }
                }

                // Set flag so we don't try to re-read generic access characteristics.
                // even if we failed this time. The service or characteristics may be missing from device.
                _genericAccessRead = true;
            }
        }

        /// <summary>
        /// Gets the address type for the Bluetooth LE device.
        /// </summary>
        public BluetoothAddressType BluetoothAddressType { get => _addressType; }

        /// <summary>
        /// Gets a boolean indicating whether the BluetoothLEDevice was paired using a Secure
        /// Connection.
        /// </summary>
        public bool WasSecureConnectionUsedForPairing { get => _pairing.WasSecureConnectionUsedForPairing; }

        /// <summary>
        /// Occurs when the connection status for the device has changed.
        /// </summary>
        public event EventHandler ConnectionStatusChanged;

        /// <summary>
        /// Occurs when the list of GATT services supported by the device has changed.
        /// </summary>
        public event EventHandler GattServicesChanged;

        /// <summary>
        /// Returns the Pairing object for the state of device paring.
        /// </summary>
        public DevicePairing Pairing { get => _pairing; }
 
        /// <summary>
        /// Utility method to read an attribute value.
        /// </summary>
        /// <param name="AttributeHandle">Handle of attribute to read.</param>
        /// <returns>Result of operation.</returns>
        internal GattReadResult ReadAttributeValue(ushort AttributeHandle)
        {
            GattCommunicationStatus status = GattCommunicationStatus.Unreachable;
            Buffer buffer = null;
            readWriteValueResult result;
            bool securityStarted = false;
            bool retryRead;

            lock (lockObj)
            {
                do
                {
                    retryRead = false;

                    //Debug.WriteLine($"# ReadAttributeValue");

                    _completedEvent.Reset();

                    //Debug.WriteLine($"# ReadValue start con:{ConnectionHandle} attrHandle:{AttributeHandle}  ");
                    result = (readWriteValueResult)NativeStartReadValue(ConnectionHandle, AttributeHandle);
                    if (result == readWriteValueResult.success)
                    {
                        // Read started, wait for completed event
                        if (_completedEvent.WaitOne(operationTimeout, false))
                        {
                            //Debug.WriteLine($"# ReadValue complete {_eventStatus} ");
                            // Read completed, check status and get value
                            if (_eventStatus == (ushort)readWriteValueResult.success)
                            {
                                buffer = new Buffer(_eventValue);
                                status = GattCommunicationStatus.Success;
                            }
                            else
                            {
                                //Debug.WriteLine($"# ReadAttributeValue error {_eventStatus}");
                                result = (readWriteValueResult)_eventStatus;
                                switch (result)
                                {
                                    case readWriteValueResult.acessDenied:
                                        if (!securityStarted)
                                        {
                                            securityStarted = true;

                                            //Debug.WriteLine($"# Access error - Start pairing");

                                            // Try to pair devices
                                            Pairing.Pair();

                                             // retry read
                                            retryRead = true;
                                            continue;
                                        }
                                        break;

                                    default:
                                        break;

                                }
                            }
                        }
                    }
                } while (retryRead) ;
            } // lock

            return new GattReadResult(buffer, status, (byte)result);
        }

        /// <summary>
        /// Utility method to write with or without a response.
        /// </summary>
        /// <param name="attributeHandle">Attribute handle to write.</param>
        /// <param name="value">Buffer value to write.</param>
        /// <param name="writeOption">Option with or without response.</param>
        /// <returns>Result of operation.</returns>
        internal GattWriteResult WriteAttributeValueWithResult(ushort attributeHandle, Buffer value, GattWriteOption writeOption = GattWriteOption.WriteWithoutResponse)
        {
            GattCommunicationStatus status = GattCommunicationStatus.Unreachable;
            readWriteValueResult result = 0;
            bool securityStarted = false;


            // Only allow 1 simultaneous call per device connection
            lock (lockObj)
            {
                do
                {
                    result = (readWriteValueResult)NativeStartWriteValue(ConnectionHandle, attributeHandle, writeOption == GattWriteOption.WriteWithResponse, value.Data, (ushort)value.Length);
                    if (result == readWriteValueResult.success)
                    {
                        if (writeOption == GattWriteOption.WriteWithResponse)
                        {
                            // Wait for response event ?
                            if (_completedEvent.WaitOne(operationTimeout, false))
                            {
                                //Debug.WriteLine($"# WriteValue with response complete {_eventStatus} ");
                                // Read completed, check status and get value
                                if (_eventStatus == (ushort)readWriteValueResult.success)
                                {
                                    status = GattCommunicationStatus.Success;
                                }
                                else
                                {
                                    result = (readWriteValueResult)_eventStatus;
                                    switch (result)
                                    {
                                        case readWriteValueResult.acessDenied:
                                            if (!securityStarted)
                                            {
                                                securityStarted = true;
                                                Pairing.Pair();

                                                // retry read
                                                continue;
                                            }
                                            break;

                                        default:
                                            break;

                                    }
                                }
                            }
                            else
                            {
                                //Debug.WriteLine($"# WriteValue timeout  ");
                            }
                        }
                        else
                        {
                            status = GattCommunicationStatus.Success;
                        }
                    }
                } while (false);
            } // lock

            return new GattWriteResult(status, (byte)result);
        }

        internal static void IdleOnLastConnection()
        {
            if (BluetoothLEServer._bluetoothEventManager.LeDeviceCount == 0)
            {
                // Reset run mode
                BluetoothNanoDevice.RunMode = BluetoothNanoDevice.Mode.NotRunning;
            }
        }

        /// <summary>
        /// Central event handler from native code.
        /// </summary>
        /// <param name="btEvent">Central event structure.</param>
        internal void OnEvent(BluetoothEventCentral btEvent)
        {
            //Debug.WriteLine($"# BluetoothLEdevcie OnEvent, type:{btEvent.type} status:{btEvent.status}");

            switch (btEvent.type)
            {
                case BluetoothEventType.ConnectComplete:

                    //Debug.WriteLine($"# OnEvent BluetoothEventType.ConnectComplete status:{btEvent.status} routing handle:{btEvent.connectionHandle} handle:{btEvent.characteristicHandle}");
                    if (btEvent.status == 0)
                    {
                        _connectionStatus = BluetoothConnectionStatus.Connected;
                        _connectionHandle = btEvent.characteristicHandle;
                    }

                    // Signal complete
                    _eventComplete.Set();

                    // Update connection status & fire event
                    ConnectionStatus = BluetoothConnectionStatus.Connected;

                    break;

                case BluetoothEventType.ConnectionDisconnected:

                    // Device has just disconnected
                    // Update connection status & fire event
                    ConnectionStatus = BluetoothConnectionStatus.Disconnected;

                    // Remove  device from event handler
                    BluetoothLEServer._bluetoothEventManager.RemoveLeDevice(this);

                    NativeDispose(_connectionHandle);

                    Pairing.Reset();

                    IdleOnLastConnection();

                    break;

                case BluetoothEventType.ServiceDiscovered:
                    {
                        //Debug.WriteLine($"# OnEvent BluetoothEventType.ServiceDiscovered ");

                        if (btEvent.status == 0)
                        {
                            // Pass GattDeviceService object to native so it can be updated as per event
                            GattDeviceService ds = new(this, btEvent.serviceHandle);

                            NativeUpdateService(btEvent.connectionHandle, ds);

                            UpdateOrAddService(ds);
                            AttributeAdd(ds);

                            //Debug.WriteLine($"# GattDeviceService handle:{ds.AttributeHandle} uuid:{ds.Uuid} start:{ds.StartHandle} end:{ds.EndHandle}");
                        }
                    }
                    break;

                case BluetoothEventType.ServiceDiscoveryComplete:

                    //Debug.WriteLine($"# GattDeviceService discovery complete, connection:{btEvent.connectionHandle} status:{btEvent.status}");

                    // Signal complete
                    _eventComplete.Set();
                    break;

                case BluetoothEventType.CharacteristicDiscovered:
                case BluetoothEventType.CharacteristicDiscoveryComplete:
                    {
                        //Debug.WriteLine($"# CharacteristicDiscovered {btEvent.serviceHandle} {btEvent.characteristicHandle}");

                        // Redirect event to correct GattDeviceService
                        GattDeviceService ds = (GattDeviceService)FindAttribute(btEvent.serviceHandle);

                        ds.OnEvent(btEvent);
                    }
                    break;

                case BluetoothEventType.AttributeReadValueComplete:

                    _eventStatus = btEvent.status;

                    //Debug.WriteLine($"# BluetoothEventType.AttributeReadValueComplete status:{_eventStatus} att:{btEvent.characteristicHandle}");
                    if (btEvent.status == 0)
                    {
                        // value available, read it 
                        _eventValue = NativeReadValue(btEvent.connectionHandle, btEvent.characteristicHandle);
                        //Debug.WriteLine($"# NativeReadValue return {_eventValue.Length}");
                    }

                    //Debug.WriteLine($"# NativeReadValue _completedEvent.Set");
                    _completedEvent.Set();
                    break;

                case BluetoothEventType.AttributeWriteValueComplete:
                    _eventStatus = btEvent.status;
                    //Debug.WriteLine($"# BluetoothEventType.CharacteristicWriteValueComplete status:{_eventStatus} sid:{btEvent.serviceHandle} chrH:{btEvent.characteristicHandle}");
                    _completedEvent.Set();
                    break;

                // Characteristic / descriptor events
                default:
                    {
                        // Redirect event to correct Characteristic
                        GattCharacteristic gc = (GattCharacteristic)FindAttribute(btEvent.characteristicHandle);

                        //Debug.WriteLine($"# Attribute event, type:{btEvent.type} status:{btEvent.status} handle:{btEvent.characteristicHandle} Found:{gc != null}");

                        if (gc != null)
                        {
                            gc.OnEvent(btEvent);
                        }
                    }
                    break;
            }
        }

        internal void OnEvent(BluetoothEventSesssion btEvent)
        {
            //Debug.WriteLine($"# BluetoothLEdevice OnEvent session, {btEvent.type} status:{btEvent.status}");

            _pairing.OnEvent(btEvent);
        }

        private void UpdateOrAddService(GattDeviceService ds)
        {
            for (int index = 0; index < _services.Count; index++)
            {
                GattDeviceService cds = (GattDeviceService)_services[index];
                if (cds.Uuid.Equals(ds.Uuid))
                {
                    // Replace service
                    _services[index] = ds;
                    return;
                }
            }

            // Add new service
            _services.Add(ds);
        }

        internal void AttributeAdd(IGattAttribute ga)
        {
            if (!_attributes.Contains(ga.AttributeHandle))
            {
                //Debug.WriteLine($"# AttributeAdd {ga.AttributeHandle}");
                _attributes.Add(ga.AttributeHandle, ga);
            }
            else
            {
                //Debug.WriteLine($"# AttributeAdd already exists {ga.AttributeHandle}");
            }
        }
        internal void AttributeRemove(ushort handle)
        {
            if (_attributes.Contains(handle))
            {
                _attributes.Remove(handle);
            }
        }

        internal IGattAttribute FindAttribute(ushort handle)
        {
            if (_attributes.Contains(handle))
            {
                return (IGattAttribute)_attributes[handle];
            }

            return null;
        }

        /// <summary>
        /// Dispose BluetoothLEDevice
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    //
                }

                // Terminate connection if still connected
                DisconnectDevice();

                _services = null;
                _attributes = null;
                _disposed = true;

                // Clean up on native side
                // if last connection then stop stack
                NativeDispose(_connectionHandle);

                // Make sure we are removed from event listener
                BluetoothLEServer._bluetoothEventManager.RemoveLeDevice(this);

                IdleOnLastConnection();
            }
        }

        /// <summary>
        /// Finalizer - dispose
        /// </summary>
        ~BluetoothLEDevice()
        {
            Dispose(disposing: false);
        }

        /// <summary>
        /// Dispose BluetoothLEDevice
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put clean up code in "Dispose(bool disposing)" method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeConnect(ulong address, BluetoothAddressType addressType, ushort eventRouting);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDisconnect(ushort conn_handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeDispose(ushort conn_handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeDiscoverServices(ushort conn_handle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeUpdateService(ushort conn_handle, GattDeviceService ds);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeStartReadValue(ushort connection, ushort valueHandle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern byte[] NativeReadValue(ushort connection, ushort valueHandle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeStartWriteValue(ushort connection, ushort valueHandle, bool withResponse, byte[] value, ushort dataLength);

        #endregion
    }
}
