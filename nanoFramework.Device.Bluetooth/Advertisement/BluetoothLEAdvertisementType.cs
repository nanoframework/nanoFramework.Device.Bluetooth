//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Specifies the different types of Bluetooth LE advertisement payloads.
    /// </summary>
    public enum BluetoothLEAdvertisementType
    {
        /// <summary>
        /// The advertisement is undirected and indicates that the device is connect-able
        /// and scan-able. This advertisement type can carry data.
        /// </summary>
        ConnectableUndirected = 0,

        /// <summary>
        /// The advertisement is directed and indicates that the device is connect able but
        /// not scan able. This advertisement type cannot carry data.
        /// </summary>
        ConnectableDirected = 1,

        /// <summary>
        /// The advertisement is undirected and indicates that the device is scan-able but
        /// not connect-able. This advertisement type can carry data.
        /// </summary>
        ScannableUndirected = 2,

        /// <summary>
        /// The advertisement is undirected and indicates that the device is not connect-able
        /// nor scan-able. This advertisement type can carry data.
        /// </summary>
        NonConnectableUndirected = 3,

        /// <summary>
        /// This advertisement is a scan response to a scan request issued for a scan-able
        /// advertisement. This advertisement type can carry data.
        /// </summary>
        ScanResponse = 4
    }
}
