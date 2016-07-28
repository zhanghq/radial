using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Web
{
    /// <summary>
    /// The standard JSON output model.
    /// </summary>
    public class StdJsonOutput
    {
        /// <summary>
        /// The error code, should be 0 if no error.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("error", Order = 1)]
        public int Error { get; set; }
        /// <summary>
        /// The message text.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("message", Order = 2)]
        public string Message { get; set; }

        /// <summary>
        /// The payload object.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("payload", Order = 3)]
        public object Payload { get; set; }

        /// <summary>
        /// To JSON string.
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            return Radial.Serialization.JsonSerializer.Serialize(this);
        }
    }
}
