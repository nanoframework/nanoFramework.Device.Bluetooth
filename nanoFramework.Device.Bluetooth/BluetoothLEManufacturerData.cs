//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// A Bluetooth LE manufacturer-specific data section (one particular type of LE
    /// advertisement section). A Bluetooth LE advertisement packet can contain multiple
    /// instances of these BluetoothLEManufacturerData objects.
    /// </summary>
    public class BluetoothLEManufacturerData
    {
        private Buffer _data;
        private ushort _companyId;

        /// <summary>
        /// Creates a new BluetoothLEManufacturerData object.
        /// </summary>
        public BluetoothLEManufacturerData() : this(0, new Buffer(1) )
        {
        }

        /// <summary>
        /// Creates a new BluetoothLEManufacturerData object with a company identifier code
        /// and manufacturer-specific section data.
        /// </summary>
        /// <param name="companyId">
        /// The Bluetooth LE company identifier code as defined by the Bluetooth Special
        /// Interest Group (SIG).
        /// </param>
        /// <param name="data">
        /// Bluetooth LE manufacturer-specific section data.
        /// </param>
        public BluetoothLEManufacturerData(ushort companyId, Buffer data)
        {
            _companyId = companyId;
            _data = data;
        }

        /// <summary>
        /// Bluetooth LE manufacturer-specific section data.
        /// </summary>
        public Buffer Data { get => _data; set => _data = value; }

        /// <summary>
        /// The Bluetooth LE company identifier code as defined by the Bluetooth Special
        /// Interest Group (SIG).
        /// </summary>
        public ushort CompanyId { get => _companyId; set => _companyId = value; }
    }
}