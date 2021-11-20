//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  Represents the value of a single Presentation Format GATT Descriptor.
    /// </summary>
    public sealed class GattPresentationFormat //: IGattPresentationFormat
    {
        private readonly byte _formatType;
        private readonly int _exponent;
        private readonly ushort _unit;
        private readonly byte _namespaceId;
        private readonly ushort _description;


        /// <summary>
        /// Creates a GattPresentationFormat object from parts.
        /// </summary>
        /// <param name="formatType">The Format Type.</param>
        /// <param name="exponent">The exponent.</param>
        /// <param name="unit">The unit.</param>
        /// <param name="namespaceId">The namespace ID.</param>
        /// <param name="description">The description.</param>
        /// <returns>An instance of GattPresentationFormat.</returns>
        public static GattPresentationFormat FromParts(byte formatType, int exponent, ushort unit, byte namespaceId, ushort description)
        {

            return new GattPresentationFormat(formatType, exponent, unit, namespaceId, description);
        }

        internal GattPresentationFormat(byte formatType, int exponent, ushort unit, byte namespaceId, ushort description)
        {
            _formatType = formatType;
            _exponent = exponent;
            _unit = unit;
            _namespaceId = namespaceId;
            _description = description;
        }

        /// <summary>
        /// Gets the Description of the GattPresentationFormat object.
        /// </summary>
        public ushort Description { get => _description; }

        /// <summary>
        /// Gets the Exponent of the GattPresentationFormat object.
        /// </summary>
        public int Exponent { get => _exponent; }

        /// <summary>
        /// Gets the Format Type of the GattPresentationFormat object.
        /// </summary>
        public byte FormatType { get => _formatType; }

        /// <summary>
        /// Gets the Name space of the GattPresentationFormat object.
        /// </summary>
        public byte Namespace { get => _namespaceId; }

        /// <summary>
        /// Gets the Unit of the GattPresentationFormat object.
        /// </summary>
        public ushort Unit { get => _unit; }
    }
}