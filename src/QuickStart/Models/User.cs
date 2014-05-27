using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//For Demonstrate Only
namespace QuickStart.Models
{
    /// <summary>
    /// User
    /// </summary>
    public class User : ModelBase
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public string Email { get; set; }
    }
}
