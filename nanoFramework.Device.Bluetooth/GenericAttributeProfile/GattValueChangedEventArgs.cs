//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

using System;

namespace nanoFramework.Device.Bluetooth.GenericAttributeProfile
{
    /// <summary>
    /// Represents the arguments received by a GattCharacteristic.ValueChanged event
    /// handler used to process characteristic value change notification and indication
    /// events sent by a Bluetooth LE device.
    /// </summary>
    public class GattValueChangedEventArgs 
    {
        Buffer _buffer;
        DateTime _timeStammp;

        internal GattValueChangedEventArgs(Buffer value, DateTime timeStammp)
        {
            _buffer = value;
            _timeStammp = timeStammp;
        }

        /// <summary>
        /// Gets the new Characteristic Value.
        /// </summary>
        public Buffer CharacteristicValue { get => _buffer; }

        /// <summary>
        /// Gets the time at which the system was notified of the Characteristic Value change.
        /// </summary>
        public DateTime Timestamp { get => _timeStammp; }
    }
}