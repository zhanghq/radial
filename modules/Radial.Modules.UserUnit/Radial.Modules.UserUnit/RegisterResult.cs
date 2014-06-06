using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Modules.UserUnit
{
    /// <summary>
    /// RegisterResult.
    /// </summary>
    public struct RegisterResult
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        public RegisterResultCode Code { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        public User User { get; set; }
    }


    /// <summary>
    /// RegisterResultCode.
    /// </summary>
    public enum RegisterResultCode
    {
        /// <summary>
        /// The OK.
        /// </summary>
        OK=0,
        /// <summary>
        /// The duplicated name.
        /// </summary>
        DuplicatedName=-1,
        /// <summary>
        /// The duplicated email.
        /// </summary>
        DuplicatedEmail=-2,
        /// <summary>
        /// The duplicated mobile.
        /// </summary>
        DuplicatedMobile=-3
    }
}
