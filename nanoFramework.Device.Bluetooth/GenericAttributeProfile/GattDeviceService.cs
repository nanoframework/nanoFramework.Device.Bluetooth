//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents a GATT Primary Service on a Bluetooth device. The GattDeviceService
    /// class represents a GATT service on a Bluetooth LE device. 
    /// </summary>
    public class GattDeviceService : IGattAttribute, IDisposable
    {
        private readonly ushort _attributeHandle;
        private readonly BluetoothLEDevice _device;
        private readonly ArrayList _characteristics;
        private readonly byte[] _uuid;

        // Start & End handles for associated attributes
        private readonly ushort _startHandle;
        private readonly ushort _endHandle;

        private AutoResetEvent _eventComplete;

        internal GattDeviceService(BluetoothLEDevice device, ushort attributeHandle)
        {
            // Keep reference to attached device
            _device = device;

            _attributeHandle = attributeHandle;

            // Assign to stop build errors
            _startHandle = 0;
            _endHandle = 0;

            // unique handle to service
            _uuid = new byte[16];
            _characteristics = new();
            _eventComplete = new AutoResetEvent(false);
        }

        /// <summary>
        /// Get the start handle for all attributes in this service.
        /// </summary>
        internal ushort StartHandle { get => _startHandle; }

        /// <summary>
        /// Get the end handle for all attributes in this service.
        /// </summary>
        internal ushort EndHandle { get => _endHandle; }

        /// <summary>
        /// Gets the handle used to uniquely identify GATT-based characteristic attributes
        /// as declared on the Bluetooth LE device.
        /// </summary>
        public ushort AttributeHandle { get => _attributeHandle; }

        /// <summary>
        /// Dispose GattDeviceService object.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Gets the characteristics that are part of this GattDeviceService instance.
        /// </summary>
        /// <returns>GattCharacteristicsResult</returns>
        public GattCharacteristicsResult GetCharacteristics()
        {
            GattCommunicationStatus status = GattCommunicationStatus.Success;

            lock (_device.lockObj)
            {
                // Only Characteristics from device first time
                if (_characteristics.Count == 0)
                {
                    try
                    {
                        _eventComplete.Reset();

                        // Start discovery of Gatt Services on Device and wait for events
                        //Debug.WriteLine($"start NativeDiscoverCharacteristics handles start:{_startHandle}  end:{_endHandle}");
                        ushort error = NativeDiscoverCharacteristics(_device.ConnectionHandle, _attributeHandle, _startHandle, _endHandle);
                        //Debug.WriteLine($"exit NativeDiscoverCharacteristics {error}");
                        if (error == 0)
                        {
                            //Debug.WriteLine($"GetCharacteristics WaitOne {_attributeHandle}");

                            if (_eventComplete.WaitOne(20000, true))
                            {
                                //Debug.WriteLine($"wait GetCharacteristics complete");

                                status = GattCommunicationStatus.Success;
                            }
                            else
                            {
                                //Debug.WriteLine($"wait GetCharacteristics timeout");
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
            }

            return new GattCharacteristicsResult(0, _characteristics, status);
        }

        /// <summary>
        /// Gets the characteristics that are part of this GattDeviceService instance and
        /// associated with the characteristicUuid.
        /// </summary>
        /// <param name="characteristicUuid">The UUID for the characteristics to be retrieved.</param>
        /// <returns>An GattCharacteristicsResult object.</returns>
        public GattCharacteristicsResult GetCharacteristicsForUuid(Guid characteristicUuid)
        {
            GattCommunicationStatus status = GattCommunicationStatus.Success;
            byte protocolError = 0;
            ArrayList selectedChars = new();

            if (_characteristics.Count == 0)
            {
                GattCharacteristicsResult result = GetCharacteristics();
                status = result.Status;
                protocolError = result.ProtocolError;
            }

            if (status == GattCommunicationStatus.Success)
            {
                foreach (GattCharacteristic chr in _characteristics)
                {
                    if (characteristicUuid.Equals(chr.Uuid))
                    {
                        selectedChars.Add(chr);
                    }
                }
            }
            return new GattCharacteristicsResult(protocolError, selectedChars, status);
        }

        /// <summary>
        ///  Gets the GATT Service UUID associated with this **GattDeviceService**.
        /// </summary>
        public Guid Uuid { get => new(_uuid); }

        /// <summary>
        /// Gets the BluetoothLEDevice object describing the device associated with the current
        /// GattDeviceService object.
        /// </summary>
        public BluetoothLEDevice Device { get => _device; }

        internal void OnEvent(BluetoothEventCentral btEvent)
        {
            switch (btEvent.type)
            {
                case BluetoothEventType.CharacteristicDiscovered:

                    //Debug.WriteLine($"# CharacteristicDiscovered, connection:{btEvent.connectionHandle} status:{btEvent.status} Service:{btEvent.serviceHandle} characteristic handle{btEvent.characteristicHandle}");

                    GattCharacteristic dc = new(this, btEvent.characteristicHandle);

                    NativeUpdateCharacteristic(_device.ConnectionHandle, AttributeHandle, btEvent.characteristicHandle, dc);

                    _characteristics.Add(dc);
                    _device.AttributeAdd(dc);
                    break;

                case BluetoothEventType.CharacteristicDiscoveryComplete:

                    //Debug.WriteLine($"# CharacteristicDiscoveryComplete, connection:{btEvent.connectionHandle} status:{btEvent.status}");

                    _eventComplete.Set();
                    break;
            }
        }

        internal GattCharacteristic FindOrAddCharacteristic(ushort handle)
        {
            GattCharacteristic c;

            foreach (object obj in _characteristics)
            {
                c = obj as GattCharacteristic;
                if (c.AttributeHandle == handle)
                {
                    return c;
                }
            }

            c = new GattCharacteristic(this, handle);
            _characteristics.Add(c);
            return c;

        }

        /// <summary>
        /// Returns the last handle used by a GattCharacteristic by either using the End handle in service or
        /// the next GattCharacteristic start value - 1
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        internal ushort GetLastHandleForCharacteristic(GattCharacteristic chr)
        {
            ushort handle = _endHandle;

            int index = _characteristics.IndexOf(chr);
            if (index < (_characteristics.Count - 1))
            {
                handle = (ushort)(((GattCharacteristic)_characteristics[index + 1]).AttributeHandle - 1);
            }

            return handle;
        }

        #region external calls to native implementations

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern ushort NativeDiscoverCharacteristics(ushort connectHandle, ushort serviceHandle, ushort startHandle, ushort endHandle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void NativeUpdateCharacteristic(ushort connectHandle, ushort serviceHandle, ushort characteriticHandle, GattCharacteristic dc);

        #endregion
    }
}