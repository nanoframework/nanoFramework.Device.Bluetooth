//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class defines the parameters of a descriptor.
    /// </summary>
    public class GattLocalDescriptorParameters 
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattProtectionLevel _writeProtectionLevel;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattProtectionLevel _readProtectionLevel;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _staticValue = null;

        /// <summary>
        /// Creates a new GattLocalDescriptorParameters object.
        /// </summary>
        public GattLocalDescriptorParameters()
        {
            _writeProtectionLevel = GattProtectionLevel.Plain;
            _readProtectionLevel = GattProtectionLevel.Plain;
        }

        /// <summary>
        /// Gets or sets the write protection level.
        /// </summary>
        public GattProtectionLevel WriteProtectionLevel { get => _writeProtectionLevel; set => _writeProtectionLevel = value; }

        /// <summary>
        /// Gets or sets the static value.
        /// </summary>
        public Buffer StaticValue { get => _staticValue; set => _staticValue = value; }

        /// <summary>
        /// Gets or sets the read protection level.
        /// </summary>
        public GattProtectionLevel ReadProtectionLevel { get => _readProtectionLevel; set => _readProtectionLevel = value; }
    }
}