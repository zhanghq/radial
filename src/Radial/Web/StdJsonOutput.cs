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
        /// The status code, should be 200 if no error.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("code", Order = 1)]
        public int Code { get; set; } = 200;
        /// <summary>
        /// The message text.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("msg", Order = 2, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Message { get; set; }

        /// <summary>
        /// The payload data.
        /// </summary>
        [Newtonsoft.Json.JsonProperty("data", Order = 3, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public object Data { get; set; }

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
