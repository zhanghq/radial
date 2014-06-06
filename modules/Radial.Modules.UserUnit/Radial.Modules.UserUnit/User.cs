using Radial.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Modules.UserUnit
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the mobile.
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt.
        /// </summary>
        protected string Salt { get; set; }

        /// <summary>
        /// Gets or sets the register time.
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// Gets or sets the register ip.
        /// </summary>
        public string RegisterIp { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        internal int Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete.
        /// </summary>
        internal bool IsDelete { get; set; }

        /// <summary>
        /// Sets the password.
        /// </summary>
        /// <param name="password">The password.</param>
        internal void SetPassword(string password)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(password), "password can not be empty or null");

            if (string.IsNullOrWhiteSpace(Salt))
                Salt = RandomCode.Create(8);

            Password = CryptoProvider.SHA1Encrypt(CryptoProvider.SHA1Encrypt(password) + Salt);
        }


        /// <summary>
        /// Determines whether [is password valid] [the specified password].
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        internal bool IsPasswordValid(string password)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(password), "password can not be empty or null");

            return string.Compare(Password, CryptoProvider.SHA1Encrypt(CryptoProvider.SHA1Encrypt(password) + Salt), true) == 0;
        }
    }
}
