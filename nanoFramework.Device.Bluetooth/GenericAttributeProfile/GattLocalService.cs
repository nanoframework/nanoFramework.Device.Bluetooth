//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class represents a GATT local service.
    /// </summary>
    public sealed class GattLocalService
    {
        private readonly byte[] _serviceUuid;
        private readonly ArrayList _characteristics;

        internal GattLocalService(Guid serviceUuid)
        {
            _serviceUuid = serviceUuid.ToByteArray();
            _characteristics = new ArrayList();
        }

        /// <summary>
        /// Creates a characteristic for this local service.
        /// </summary>
        /// <param name="characteristicUuid">The characteristic UUID.</param>
        /// <param name="parameters">The characteristic parameters.</param>
        /// <returns>An GattLocalCharacteristicResult object</returns>
        public GattLocalCharacteristicResult CreateCharacteristic(Guid characteristicUuid, GattLocalCharacteristicParameters parameters)
        {
            GattLocalCharacteristic characteristic = new GattLocalCharacteristic(characteristicUuid, parameters);
            _characteristics.Add(characteristic);

            return new GattLocalCharacteristicResult(characteristic, BluetoothError.Success);
        }

        /// <summary>
        /// Gets a array of the characteristics available for this local service.
        /// </summary>
        public GattLocalCharacteristic[] Characteristics { get { return (GattLocalCharacteristic[])_characteristics.ToArray(typeof(GattLocalCharacteristic)); } }

        /// <summary>
        ///  Gets the local service UUID.
        /// </summary>
        public Guid Uuid { get => new Guid(_serviceUuid); }
    }
}