//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// A representation of a Bluetooth LE advertisement payload.
    /// </summary>
    public class BluetoothLEAdvertisement
    {
        private BluetoothLEAdvertisementFlags _flags;
        private string _localName;
        private Guid[] _serviceUuids;
        private BluetoothLEAdvertisementDataSection[] _dataSections;
        private ArrayList _manufacturerData;

        // Used to import the byte arrays from Native code
        private byte[] _rawUuids;
        private byte[] _rawManufacturerData;

        /// <summary>
        /// Creates a new BluetoothLEAdvertisement object.
        /// </summary>
        public BluetoothLEAdvertisement()
        {
            _serviceUuids = new Guid[0];
            _manufacturerData = new ArrayList();
            _localName = "";
            _dataSections = null;
            _flags = BluetoothLEAdvertisementFlags.ClassicNotSupported | BluetoothLEAdvertisementFlags.GeneralDiscoverableMode;
        }

        /// <summary>
        /// Return an ArrayList of advertisement data sections that matches a given advertisement
        /// section type in a BluetoothLEAdvertisement.
        /// </summary>
        /// <param name="type">The advertisement section type</param>
        /// <returns>
        /// An ArrayList of all the BluetoothLEAdvertisementDataSection matching the given advertisement
        /// type. This method returns an empty list if no such sections are found in the
        /// payload.
        /// </returns>
        public ArrayList GetSectionsByType(byte type)
        {
            // Return array containing sections
            ArrayList retArray = new();

            foreach (BluetoothLEAdvertisementDataSection s in _dataSections)
            {
                if (s.DataType == type)
                {
                    retArray.Add(s);
                }
            }

            return retArray;
        }

        /// <summary>
        /// The local name contained within the advertisement. This property can be either
        /// the shortened or complete local name defined by the Bluetooth LE specifications.
        /// </summary>
        public string LocalName { get => _localName; set => _localName = value.Trim(); }

        /// <summary>
        /// Bluetooth LE advertisement flags.
        /// </summary>
        public BluetoothLEAdvertisementFlags Flags { get => _flags; set => _flags = value; }

        /// <summary>
        /// Gets the arrayList of raw data sections.
        /// </summary>
        public ArrayList DataSections
        {
            get
            {
                ArrayList retList = new();
                foreach (BluetoothLEAdvertisementDataSection s in _dataSections)
                {
                    retList.Add(s);
                }
                return retList;
            }
        }

        /// <summary>
        /// Process the RawManufacturerData bytes into array when available
        /// </summary>
        private void ProcessRawManufacturerData()
        {
            if (_rawManufacturerData != null && _rawManufacturerData.Length > 0)
            {
                _manufacturerData.Clear();

                // first 2 bytes are company id
                ushort CompanyID = (ushort)(_rawManufacturerData[0] + (_rawManufacturerData[1] << 8));

                // Rest are the Manufacturers Data
                byte[] mandata = new byte[_rawManufacturerData.Length - 2];
                Array.Copy(_rawManufacturerData, 2, mandata, 0, _rawManufacturerData.Length - 2);

                Buffer data = new(mandata);
                _manufacturerData.Add(new BluetoothLEManufacturerData(CompanyID, data));
                _rawManufacturerData = null;
            }
        }

        /// <summary>
        /// Gets the list of manufacturer-specific data sections in a BluetoothLEAdvertisement.
        /// </summary>
        public ArrayList ManufacturerData
        {
            get
            {
                ProcessRawManufacturerData();
                return _manufacturerData;
            }
        }

        /// <summary>
        /// Return a list of all manufacturer data sections in the BluetoothLEAdvertisement
        /// payload matching the specified company id.
        /// </summary>
        /// <param name="companyId">The company identifier code defined by the Bluetooth Special Interest Group (SIG).</param>
        /// <returns>ArrayList of BluetoothLEManufacturerData by companyId</returns>
        public ArrayList GetManufacturerDataByCompanyId(ushort companyId)
        {
            ArrayList subset = new();

            // Process if not already done
            ProcessRawManufacturerData();

            foreach (BluetoothLEManufacturerData obj in _manufacturerData)
            {
                if (obj.CompanyId == companyId)
                {
                    subset.Add(obj);
                }
            }

            return subset;
        }

        /// <summary>
        /// The list of service UUIDs in 128-bit GUID format. This property 
        /// the 16-bit, 32-bit and 128-bit service UUIDs into a single list.
        /// </summary>
        public Guid[] ServiceUuids
        {
            get
            {
                if (_rawUuids != null && _rawUuids.Length > 0)
                {
                    byte[] uuid = new byte[16];

                    // First time Parse Import array into guid array
                    // linear byte[] with records of 16 bytes per UUID
                    int numberUUIDS = _rawUuids.Length / 16;
                    _serviceUuids = new Guid[numberUUIDS];
                    for (int n = 0, i = 0; n < numberUUIDS; i += 16, n++)
                    {
                        Array.Copy(_rawUuids, i, uuid, 0, 16);
                        _serviceUuids[n] = new Guid(uuid);
                    }
                    _rawUuids = null;
                }
                return _serviceUuids;
            }
        }
    }
}