//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.Advertisement
{
    /// <summary>
    /// Defines constants that specify a Bluetooth LE scanning mode.
    /// </summary>
    public enum BluetoothLEScanningMode
    {
        /// <summary>
        /// Specifies a passive scanning mode. This is the default scanning mode.
        /// </summary>
        Passive = 0,

        /// <summary>
        /// Specifies an active scanning mode. This indicates that scan request packets will be sent from the 
        /// platform to actively query for more advertisement data of type BluetoothLEAdvertisementType.ScanResponse.
        /// </summary>
        Active = 1
    }
}
