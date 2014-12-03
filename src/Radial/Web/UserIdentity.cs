using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.Web.Security;

namespace Radial.Web
{
    /// <summary>
    /// A class contains user data.
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// Gets or sets the user id (ensure the value can be serialized and deserialized with JSON format correctly).
        /// </summary>
        public dynamic Id { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the additional data of user (ensure the value can be serialized and deserialized with JSON format correctly).
        /// </summary>
        public dynamic Others { get; set; }


        /// <summary>
        /// Gets a value that indicates whether the user has been authenticated.
        /// </summary>
        [JsonIgnore]
        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets the type of authentication used.
        /// </summary>
        [JsonIgnore]
        public string AuthenticationType
        {
            get
            {
                return HttpContext.Current.User.Identity.AuthenticationType;
            }
        }

        /// <summary>
        /// Gets the user of the current principal.
        /// </summary>
        /// <exception cref="System.Reflection.TargetInvocationException">Can not deserialize to UserIdentity instance from the user data string saved in JSON format.</exception>
        public static UserIdentity Current
        {
            get
            {
                FormsIdentity identity = HttpContext.Current.User.Identity as FormsIdentity;
                if (identity == null || string.IsNullOrWhiteSpace(identity.Ticket.UserData))
                    return new UserIdentity { Name = HttpContext.Current.User.Identity.Name };
                return JsonConvert.DeserializeObject<UserIdentity>(identity.Ticket.UserData);
            }
        }
    }
}
