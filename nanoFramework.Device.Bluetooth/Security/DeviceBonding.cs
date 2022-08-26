//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.Security
{
    /// <summary>
    /// Class to encapsulate the bond store access.
    /// </summary>
    public static class DeviceBonding
    {
        /// <summary>
        /// Is passed bluetooth address bonded
        /// </summary>
        /// <param name="bluetoothAddress"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsBonded(ulong bluetoothAddress);

        /// <summary>
        /// Delete all stored bonds.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void DeleteAllBonds();

        /// <summary>
        /// Delete bond information for peer.
        /// </summary>
        /// <param name="peerbluetoothAddress">Bluetooth address of Peer</param>
        /// <param name="addressType">Type of Bluetooth address</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void DeleteBondForPeer(ulong peerbluetoothAddress, BluetoothAddressType addressType);

        /// <summary>
        /// Returns the number of stored bonds.
        /// </summary>
        /// <returns>Bond count.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern int Count();

        /// <summary>
        /// Get Bonding address for peer at index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern ulong GetBondInformationAt(int index);
    }
}
