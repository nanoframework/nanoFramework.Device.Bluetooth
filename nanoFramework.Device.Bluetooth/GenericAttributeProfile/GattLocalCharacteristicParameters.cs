//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System.Collections;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// This class contains the local characteristic descriptor parameters.
    /// </summary>
    public class GattLocalCharacteristicParameters
    {
        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattProtectionLevel _writeProtectionLevel;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattProtectionLevel _readProtectionLevel;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private string _userDescription;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private GattCharacteristicProperties _properties;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private Buffer _staticValue;

        [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
        private readonly ArrayList _presentationFormats;

        /// <summary>
        /// Creates a new GattLocalCharacteristicParameters object.
        /// </summary>
        public GattLocalCharacteristicParameters()
        {
            _writeProtectionLevel = GattProtectionLevel.Plain;
            _readProtectionLevel = GattProtectionLevel.Plain;
            _userDescription = "";
            _properties = GattCharacteristicProperties.None;
            _presentationFormats = new ArrayList();
        }

        /// <summary>
        /// Gets and sets the write protection level.
        /// </summary>
        public GattProtectionLevel WriteProtectionLevel { get => _writeProtectionLevel; set => _writeProtectionLevel = value; }

        /// <summary>
        /// Gets or sets the user-friendly description.
        /// </summary>
        public string UserDescription { get => _userDescription; set => _userDescription = value; }

        /// <summary>
        ///  Gets or sets the static value.
        /// </summary>
        public Buffer StaticValue { get => _staticValue; set => _staticValue = value; }

        /// <summary>
        /// Gets or sets the read protection level.
        /// </summary>
        public GattProtectionLevel ReadProtectionLevel { get => _readProtectionLevel; set => _readProtectionLevel = value; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public GattCharacteristicProperties CharacteristicProperties { get => _properties; set => _properties = value; }

        /// <summary>
        ///  Gets or sets the presentation formats arrayLIst
        /// </summary>
        public GattPresentationFormat[] PresentationFormats { get => (GattPresentationFormat[])_presentationFormats.ToArray(typeof(GattPresentationFormat)); }

        /// <summary>
        /// Create a GattPresentationFormat to the GattLocalCharacteristicParameters
        /// </summary>
        public void CreateGattPresentationFormat(byte formatType, int exponent, ushort unit, byte namespaceId, ushort decsription)
        {
            _presentationFormats.Add(new GattPresentationFormat(formatType, exponent, unit, namespaceId, decsription));
        }
    }
}

