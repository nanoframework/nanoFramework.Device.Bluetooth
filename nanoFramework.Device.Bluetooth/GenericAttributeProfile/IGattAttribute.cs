//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Interface for all attributes ( Services, Characteristics, Descriptors )
    /// </summary>
    public interface IGattAttribute
    {
        /// <summary>
        /// Unique Attribute handle.
        /// </summary>
        public ushort AttributeHandle { get;  }

        /// <summary>
        /// UUID of Attribute.
        /// </summary>
        public Guid Uuid { get; }
    }
}
