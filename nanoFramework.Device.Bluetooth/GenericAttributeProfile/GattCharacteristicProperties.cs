﻿//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Specifies the values for the GATT characteristic
    /// Extended Characteristic Properties Descriptor.
    /// </summary>
    [Flags]
    public enum GattCharacteristicProperties : uint
    {
        /// <summary>
        /// The characteristic doesn’t have any properties that apply.
        /// </summary>
        None = 0,

        /// <summary>
        /// The characteristic supports broadcasting
        /// </summary>
        Broadcast = 1,

        /// <summary>
        /// The characteristic is readable
        /// </summary>
        Read = 2,

        /// <summary>
        /// The characteristic supports Write Without Response
        /// </summary>
        WriteWithoutResponse = 4,

        /// <summary>
        /// The characteristic is writeable
        /// </summary>
        Write = 8,

        /// <summary>
        /// The characteristic is notifiable
        /// </summary>
        Notify = 16,

        /// <summary>
        /// The characteristic is indicatable
        /// </summary>
        Indicate = 32,

        /// <summary>
        /// The characteristic supports signed writes
        /// </summary>
        AuthenticatedSignedWrites = 64,

        /// <summary>
        /// The ExtendedProperties Descriptor is present
        /// </summary>
        ExtendedProperties = 128,

        /// <summary>
        /// The characteristic supports reliable writes
        /// </summary>
        ReliableWrites = 256,

        /// <summary>
        /// The characteristic has writeable auxiliaries
        /// </summary>
        WritableAuxiliaries = 512
    }
}