//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

namespace nanoFramework.Device.Bluetooth
{
    /// <summary>
    /// Object to hold password credentials
    /// </summary>
    public class PasswordCredential
    {
        private string _username;
        private string _password;

        /// <summary>
        /// Constructs a PasswordCredential
        /// </summary>
        /// <param name="userName">User name in credential</param>
        /// <param name="password">Password for user name</param>
        public PasswordCredential(string userName, string password)
        {
            _username = userName;
            _password = password;
        }

        /// <summary>
        /// Returns password from Credential
        /// </summary>
        public string Password { get { return _password; } }

        /// <summary>
        /// Returns User name from Credential
        /// </summary>
        public string UserName { get { return _username; } }
    }
}
