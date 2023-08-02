//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Groups parameters used to configure payload-based filtering of received Bluetooth
    /// LE advertisements.
    /// </summary>
    public class BluetoothLEAdvertisementFilter
    {
        private BluetoothLEAdvertisement _advertisement;
        private ArrayList _bytePatterns;

        /// <summary>
        /// Creates a new <see cref="BluetoothLEAdvertisementFilter"/> object.
        /// </summary>
        public BluetoothLEAdvertisementFilter()
        {
            _advertisement = new BluetoothLEAdvertisement();
            _bytePatterns = new ArrayList();
        }

        /// <summary>
        /// A <see cref="BluetoothLEAdvertisement"/> object that can be applied as filters to received
        /// Bluetooth LE advertisements.
        /// </summary>
        public BluetoothLEAdvertisement Advertisement { get => _advertisement; set => _advertisement = value; }

        /// <summary>
        /// Gets a arrayList of byte patterns with offsets to match advertisement sections in
        /// a received Bluetooth LE advertisement.
        /// </summary>
        public ArrayList BytePatterns { get => _bytePatterns; }

        internal bool Filter(BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // Check BluetoothLEAdvertisement filter
            foreach (BluetoothLEAdvertisementDataSection ds in _advertisement.DataSections)
            {
                // find filter data section type in advertisement
                ArrayList sections = args.Advertisement.GetSectionsByType(ds.DataType);
                if (sections.Count == 0)
                {
                    return false;
                }

                // Check data section in advertisement is same value as whats in filer
                BluetoothLEAdvertisementDataSection fds = (BluetoothLEAdvertisementDataSection)sections[0];
                if (!CompareBufferWithOffset(ds.Data, fds.Data, 0))
                {
                    return false;
                }
            }

            // Check BluetoothLEAdvertisementBytePattern filter
            foreach (BluetoothLEAdvertisementBytePattern bp in _bytePatterns)
            {
                ArrayList sections = args.Advertisement.GetSectionsByType(bp.DataType);
                if (sections.Count == 0)
                {
                    return false;
                }

                BluetoothLEAdvertisementDataSection fds = (BluetoothLEAdvertisementDataSection)sections[0];
                if (!CompareBufferWithOffset(fds.Data, bp.Data, bp.Offset))
                {
                    return false;
                }
            }

            return true;
        }

        // Check byte pattern is in byte[] at offset.
        private bool CompareByteArrayOffset(byte[] a, byte[] b, uint blen, short boffset)
        {
            // Check offset is in data
            if (a.Length < (blen + boffset))
            {
                return false;
            }

            for (int i = 0; i < blen; i++)
            {
                if (a[i] != b[i + boffset])
                {
                    return false;
                }
            }

            return true;
        }

        private bool CompareBufferWithOffset(Buffer a, Buffer b, short boffset)
        {
            // Check offset is in data
            if (a.Length < (b.Length + boffset))
            {
                return false;
            }

            for (int i = 0; i < b.Length; i++)
            {
                if (a.Data[i + boffset] != b.Data[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
