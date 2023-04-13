//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Collections;
using System.Text;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// A representation of a Bluetooth LE advertisement payload.
    /// </summary>
    public class BluetoothLEAdvertisement
    {
        private BluetoothLEAdvertisementFlags _flags;
        private string _localName;
        private ArrayList _serviceUuids;
        private ArrayList _dataSections;
        private ArrayList _manufacturerData;
        private bool _isConnectable;
        private bool _isDiscovable;

        /// <summary>
        /// Creates a new BluetoothLEAdvertisement object.
        /// </summary>
        public BluetoothLEAdvertisement()
        {
            _serviceUuids = new();
            _manufacturerData = new();
            _localName = "";
            _dataSections = new();
            _flags = BluetoothLEAdvertisementFlags.ClassicNotSupported | BluetoothLEAdvertisementFlags.GeneralDiscoverableMode;
            _isConnectable = false;
            _isDiscovable = true;
        }

        #region Properties
        /// <summary>
        /// Gets the arrayList of raw _advertData sections.
        /// </summary>
        public ArrayList DataSections
        {
            get
            {
                MergeManufacturerArray();
                return _dataSections;
            }
        }

        /// <summary>
        /// The local name contained within the advertisement. This property can be either
        /// the shortened or complete local name defined by the Bluetooth LE specifications.
        /// </summary>
        public string LocalName
        {
            get => _localName;
            set
            {
                _localName = value;

                if (_localName == null || _localName.Length == 0)
                {
                    RemoveSectionsOfType(BluetoothLEAdvertisementDataSectionType.CompleteLocalName);
                }
                else
                {
                    // Update data section.
                    DataWriter dw = new DataWriter();
                    dw.WriteString(_localName);
                    AddOrUpdateDataSection(BluetoothLEAdvertisementDataSectionType.CompleteLocalName, dw.DetachBuffer());
                }
            }
        }

        /// <summary>
        /// Bluetooth LE advertisement flags.
        /// Defaults to ClassicNotSupported and GeneralDiscoverableMode.
        /// </summary>
        public BluetoothLEAdvertisementFlags Flags
        {
            get => _flags;
            set
            {
                _flags = value;

                // Update data section with new value.
                AddOrUpdateDataSection(BluetoothLEAdvertisementDataSectionType.Flags, new Buffer(new byte[] { (byte)_flags }));
            }
        }

        /// <summary>
        /// Gets the list of manufacturer-specific sections in a BluetoothLEAdvertisement.
        /// </summary>
        public ArrayList ManufacturerData { get => _manufacturerData; }

        /// <summary>
        /// The array of service UUIDs in 128-bit GUID format. This property aggregates all 
        /// the 16-bit, 32-bit and 128-bit service UUIDs into a single array.
        /// </summary>
        public Guid[] ServiceUuids { get => (Guid[])_serviceUuids.ToArray(typeof(Guid)); }

        /// <summary>
        /// Internal property to get and set isConnectable property from service provider.
        /// </summary>
        internal bool IsConnectable { get => _isConnectable; set => _isConnectable = value; }

        /// <summary>
        /// Internal property to get and set IsDiscovable property from service provider.
        /// </summary>
        internal bool IsDiscovable { get => _isDiscovable; set => _isDiscovable = value; }

        #endregion

        #region Methods
        /// <summary>
        /// Returns an ArrayList of all the BluetoothLEAdvertisementDataSection matching the given advertisement
        /// type. This method returns an empty list if no such sections are found in the
        /// payload.        /// </summary>
        /// <param name="type">The advertisement section type</param>
        /// <returns>
        /// BluetoothLEAdvertisementDataSection ArrayList of matching advertisement types.
        /// This method returns an empty list if no such sections are found in the payload.
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
        /// Return a list of all manufacturer Data sections in the BluetoothLEAdvertisement
        /// payload matching the specified company id.
        /// </summary>
        /// <param name="companyId">The company identifier code defined by the Bluetooth Special Interest Group (SIG).</param>
        /// <returns>ArrayList of BluetoothLEManufacturerData by companyId.</returns>
        public ArrayList GetManufacturerDataByCompanyId(ushort companyId)
        {
            ArrayList subset = new();

            foreach (BluetoothLEManufacturerData obj in _manufacturerData)
            {
                if (obj.CompanyId == companyId)
                {
                    subset.Add(obj);
                }
            }

            return subset;
        }

        #endregion

        /// <summary>
        /// Add or Update section in _dataSections ArrayList.
        /// </summary>
        /// <param name="type">Section type.</param>
        /// <param name="data">Buffer with data.</param>
        internal void AddOrUpdateDataSection(BluetoothLEAdvertisementDataSectionType type, Buffer data)
        {
            BluetoothLEAdvertisementDataSection bds = new BluetoothLEAdvertisementDataSection((byte)type, data);

            for (int index = 0; index < _dataSections.Count; index++)
            {
                BluetoothLEAdvertisementDataSection ds = (BluetoothLEAdvertisementDataSection)_dataSections[index];
                if ((BluetoothLEAdvertisementDataSectionType)ds.DataType == type)
                {
                    // Replace section
                    _dataSections[index] = bds;
                    return;
                }
            }

            // Add new service
            _dataSections.Add(bds);
        }

        #region Parse raw incoming advert byte array data

        /// <summary>
        /// Parse the byte array received from native code into _advertData sections and relevant properties.
        /// </summary>
        /// <param name="bytes"></param>
        internal void ParseBytesToSectionData(byte[] bytes)
        {
            int byteIndex = 0;

            // Clear arrays
            _serviceUuids.Clear();
            _manufacturerData.Clear();

            // Each section is composed of 1 byte dataLength following _advertData, 1 byte type, Data bytes
            // TODO check dataLength 
            while (byteIndex < bytes.Length)
            {
                int dataLength = bytes[byteIndex++] - 1;
                byte dataType = bytes[byteIndex++];

                // Extract _advertData section from byte array
                byte[] dataBytes = new byte[dataLength];
                Array.Copy(bytes, byteIndex, dataBytes, 0, dataLength);
                Buffer buffer = new Buffer(dataBytes);

                byteIndex += dataLength;

                BluetoothLEAdvertisementDataSection dataSection = new BluetoothLEAdvertisementDataSection(dataType, buffer);
                _dataSections.Add(dataSection);

                LoadDataSectionToProperty(dataSection);
            }
        }

        private void LoadDataSectionToProperty(BluetoothLEAdvertisementDataSection ds)
        {
            switch ((BluetoothLEAdvertisementDataSectionType)ds.DataType)
            {
                case BluetoothLEAdvertisementDataSectionType.Flags:
                    _flags = (BluetoothLEAdvertisementFlags)ds.Data.Data[0];
                    break;

                // Local name
                case BluetoothLEAdvertisementDataSectionType.ShortenedLocalName:
                case BluetoothLEAdvertisementDataSectionType.CompleteLocalName:
                    _localName = Encoding.UTF8.GetString(ds.Data.Data, 0, ds.Data.Data.Length);
                    break;

                case BluetoothLEAdvertisementDataSectionType.ManufacturerSpecificData:
                    ParseManufacturerData(ds.Data.Data);
                    break;

                // Service UUID
                case BluetoothLEAdvertisementDataSectionType.CompleteList16uuid:
                case BluetoothLEAdvertisementDataSectionType.IncompleteList16uuid:
                case BluetoothLEAdvertisementDataSectionType.CompleteList32uuid:
                case BluetoothLEAdvertisementDataSectionType.IncompleteList32uuid:
                case BluetoothLEAdvertisementDataSectionType.CompleteList128uuid:
                case BluetoothLEAdvertisementDataSectionType.IncompleteList128uuid:
                    ParseUuidList(ds.Data.Data);
                    break;
            }
        }

        /// <summary>
        /// Load properties from DataSection array
        /// </summary>
        private void loadPropertiesFromDataSections()
        {
            foreach (BluetoothLEAdvertisementDataSection ds in _dataSections)
            {
                LoadDataSectionToProperty(ds);
            }
        }

        /// <summary>
        /// Parse raw manufacturer data into _manufacturerData arrayList
        /// </summary>
        /// <param name="rawManufacturerData"></param>
        private void ParseManufacturerData(byte[] rawManufacturerData)
        {
            // first 2 bytes are company id
            ushort CompanyID = (ushort)(rawManufacturerData[0] + (rawManufacturerData[1] << 8));

            // Rest are the Manufacturers Data
            byte[] mandata = new byte[rawManufacturerData.Length - 2];
            Array.Copy(rawManufacturerData, 2, mandata, 0, rawManufacturerData.Length - 2);

            Buffer data = new(mandata);
            _manufacturerData.Add(new BluetoothLEManufacturerData(CompanyID, data));
        }

        /// <summary>
        /// Parse byte array of UUID into service UUIDS.
        /// </summary>
        /// <param name="rawUuidData"></param>
        private void ParseUuidList(byte[] rawUuidData)
        {
            int numberUUIDS = rawUuidData.Length / 16;

            DataReader uRdr = DataReader.FromBuffer(new Buffer(rawUuidData));

            for (int n = 0, i = 0; n < numberUUIDS; i += 16, n++)
            {
                _serviceUuids.Add(uRdr.ReadUuid());
            }
        }

        #endregion

        internal void RemoveSectionsOfType(BluetoothLEAdvertisementDataSectionType sectionType)
        {
            for (int i = _dataSections.Count - 1; i >= 0; i--)
            {
                BluetoothLEAdvertisementDataSection ds = (BluetoothLEAdvertisementDataSection)_dataSections[i];
                if (ds.DataType == (byte)sectionType)
                {
                    _dataSections.RemoveAt(i);
                }
            }
        }

        private void MergeManufacturerArray()
        {
            // First remove all manufacturer data from data sections list
            RemoveSectionsOfType(BluetoothLEAdvertisementDataSectionType.ManufacturerSpecificData);

            foreach(BluetoothLEManufacturerData md in _manufacturerData)
            {
                DataWriter dw = new DataWriter();
                dw.WriteUInt16(md.CompanyId);
                for (int i = 0; i < md.Data.Length; i++)
                {
                    dw.WriteByte(md.Data.Data[i]);
                }

                BluetoothLEAdvertisementDataSection bds = new BluetoothLEAdvertisementDataSection((byte)BluetoothLEAdvertisementDataSectionType.ManufacturerSpecificData, dw.DetachBuffer());

                _dataSections.Add(bds);
            }
        }


        /// <summary>
        /// Merge the scan response advertisement into original advertisement so all data sections are together.
        /// </summary>
        /// <param name="mergeAdvert"></param>
        internal void MergeAdvertisement(BluetoothLEAdvertisement mergeAdvert)
        {
            foreach (BluetoothLEAdvertisementDataSection ds in mergeAdvert.DataSections)
            {
                _dataSections.Add(ds);
            }
            loadPropertiesFromDataSections();
        }

        /// <summary>
        /// Create a 31 byte legacy advertisement bytes.
        /// Any sections that don't fix in advertisement then add to scan response.
        /// </summary>
        /// <returns>True if data didn't fit advert or scan response.</returns>
        internal bool CreateLegacyAdvertisements(out byte[] advertData, out byte[] scanResponse)
        {
            int advertIndex = 0;
            int srIndex = 0;
            bool advertNotFit = false;

            ArrayList noFitList = new ArrayList();

            const int MAX_ADVERT_SIZE = 31;

            // Initialize buffer to max dataLength
            advertData = new byte[MAX_ADVERT_SIZE];
            scanResponse = new byte[MAX_ADVERT_SIZE];

            MergeManufacturerArray();

            // Add data sections to _advertData
            foreach (BluetoothLEAdvertisementDataSection ds in _dataSections)
            {
                int len = ds.AdvertisentLength();
                if (advertIndex + len <= MAX_ADVERT_SIZE)
                {
                    Array.Copy(ds.ToAdvertisentBytes(), 0, advertData, advertIndex, len);
                    advertIndex += len;
                }
                else
                {
                    // Add to list of items that don't fit in primary advert
                    noFitList.Add(ds);
                }
            }

            // Add data sections that didn't fit in advert to scan response
            foreach (BluetoothLEAdvertisementDataSection ds in noFitList)
            {
                int len = ds.AdvertisentLength();
                if (srIndex + len <= MAX_ADVERT_SIZE)
                {
                    Array.Copy(ds.ToAdvertisentBytes(), 0, scanResponse, srIndex, len);
                    srIndex += len;
                }
                else
                {
                    // ignore this section and set return flag
                    advertNotFit = true;
                }
            }

            // adjust arrays to correct exact size of data
            advertData = AdjustByteArraySize(advertData, advertIndex);
            scanResponse = AdjustByteArraySize(scanResponse, srIndex);

            return advertNotFit;
        }

        private byte[] AdjustByteArraySize(byte[] array, int len)
        {
            SpanByte sb = new SpanByte(array).Slice(0, len);
            return sb.ToArray();
        }
    }
}