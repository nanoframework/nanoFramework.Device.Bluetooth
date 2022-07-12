//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GattSubscribedCliensChangedEventArgs : EventArgs
    {
        private readonly bool _subscribed;
        private readonly GattSubscribedClient _client = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscribed"></param>
        /// <param name="client"></param>
        public GattSubscribedCliensChangedEventArgs(bool subscribed, GattSubscribedClient client)
        {
            _subscribed = subscribed;
            _client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Subscribed => _subscribed;

        /// <summary>
        /// 
        /// </summary>
        public GattSubscribedClient Client => _client;
    }
}
