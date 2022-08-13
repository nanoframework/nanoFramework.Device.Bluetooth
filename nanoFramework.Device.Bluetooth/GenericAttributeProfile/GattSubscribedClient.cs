//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    ///  This class represents a subscribed client of a GATT session.
    /// </summary>
    public class GattSubscribedClient
    {
        private readonly GattSession _session;

        internal GattSubscribedClient(GattSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets the maximum notification size.
        /// </summary>
        public ushort MaxNotificationSize { get => 1024; }

        /// <summary>
        /// Gets the session of the subscribed client.
        /// </summary>
        public GattSession Session { get => _session; }
    }
}
