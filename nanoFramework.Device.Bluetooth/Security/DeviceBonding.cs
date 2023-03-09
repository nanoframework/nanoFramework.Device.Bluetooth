//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;
using System.Runtime.CompilerServices;

namespace nanoFramework.Device.Bluetooth.Security
{
    /// <summary>
    /// Class to encapsulate the bond store access.
    /// </summary>
    public static class DeviceBonding
    {
        /// <summary>
        /// Check if bluetooth address bonded.
        /// </summary>
        /// <param name="bluetoothAddress">Bluetooth address to check.</param>
        /// <returns>True if bonded.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern bool IsBonded(ulong bluetoothAddress);

        /// <summary>
        /// Deletes all stored bonds.
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void DeleteAllBonds();

        /// <summary>
        /// Deletes bond information for peer.
        /// </summary>
        /// <param name="peerbluetoothAddress">Peer's Bluetooth address.</param>
        /// <param name="addressType">Type of Bluetooth address.</param>
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
        /// <param name="index">The index.</param>
        /// <returns>The bonded address.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Index out range.</exception>
        /// <exception cref="NotSupportedException">Bluetooth Stack is not running. run BluetoothLEServer.Start().</exception>        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern ulong GetBondInformationAt(int index);
    }
}
