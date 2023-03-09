//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Class to hold password credentials.
    /// </summary>
    public class PasswordCredential
    {
        private readonly string _username;
        private readonly string _password;

        /// <summary>
        /// Constructs a Password Credential.
        /// </summary>
        /// <param name="userName">User name in credential.</param>
        /// <param name="password">Password for user name.</param>
        public PasswordCredential(string userName, string password)
        {
            _username = userName;
            _password = password;
        }

        /// <summary>
        /// Gets password from Credential.
        /// </summary>
        public string Password { get => _password; }

        /// <summary>
        /// Gets User name from Credential.
        /// </summary>
        public string UserName { get => _username; }
    }
}