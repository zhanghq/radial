using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Radial;

namespace Radial.Lock
{
    /// <summary>
    /// Repersents lock data contract
    /// </summary>
    [DataContract]
    public class LockObject
    {
        string _key;

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        [DataMember]
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value == null ? value : value.Trim().ToLower();
            }
        }


        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        [DataMember]
        public DateTime CreateTime
        {
            get;set;
        }


        /// <summary>
        /// Gets or sets the expire time.
        /// </summary>
        [DataMember]
        public DateTime ExpireTime
        {
            get;set;
        }


        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return DateTime.Now > ExpireTime;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            LockObject o= obj as LockObject;
 
            if(o==null)
                return false;

            return this.GetHashCode()==o.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}
