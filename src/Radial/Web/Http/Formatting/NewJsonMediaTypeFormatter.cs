using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Radial.Web.Http.Formatting
{
    /// <summary>
    /// A <see cref="MediaTypeFormatter"/> that supports the following media types:
    /// "text/json", "application/json".
    /// </summary>
    public class NewJsonMediaTypeFormatter : JsonMediaTypeFormatter
    {
        private JsonSerializerSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonMediaTypeFormatter"/> class.
        /// </summary>
        public NewJsonMediaTypeFormatter()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewJsonMediaTypeFormatter" /> class with
        /// the specified Json serializer settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public NewJsonMediaTypeFormatter(JsonSerializerSettings settings)
        {
            _settings = settings ?? new JsonSerializerSettings();

            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/json") { CharSet = "utf-8" });
            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json") { CharSet = "utf-8" });
        }


        /// <summary>
        /// Writes an object of the specified <paramref name="type" /> to the specified <paramref name="writeStream" />. This method is called during serialization.
        /// </summary>
        /// <param name="type">The type of object to write.</param>
        /// <param name="value">The object to write.</param>
        /// <param name="writeStream">The <see cref="T:System.IO.Stream" /> to which to write.</param>
        /// <param name="content">The <see cref="T:System.Net.Http.HttpContent" /> where the content is being written.</param>
        /// <param name="transportContext">The <see cref="T:System.Net.TransportContext" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> that will write the value to the stream.
        /// </returns>
        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, System.Net.Http.HttpContent content, TransportContext transportContext)
        {
            // Create a serializer 
            JsonSerializer serializer = JsonSerializer.Create(_settings);
            
            // Create task writing the serialized content 
            return Task.Factory.StartNew(() =>
            {
                TextWriter tw = new StreamWriter(writeStream, GlobalVariables.Encoding);

                serializer.Serialize(tw, value);
                tw.Flush();
            });
        }


        /// <summary>
        /// Reads an object of the specified <paramref name="type" /> from the specified <paramref name="readStream" />. This method is called during deserialization.
        /// </summary>
        /// <param name="type">The type of object to read.</param>
        /// <param name="readStream">Thestream from which to read</param>
        /// <param name="content">The content being written.</param>
        /// <param name="formatterLogger">The <see cref="T:System.Net.Http.Formatting.IFormatterLogger" /> to log events to.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />.
        /// </returns>
        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger)
        {
            // Create a serializer 
            JsonSerializer serializer = JsonSerializer.Create(_settings);

            // Create task reading the content 
            return Task.Factory.StartNew(() =>
            {
                TextReader tr = new StreamReader(readStream, GlobalVariables.Encoding);

                return serializer.Deserialize(tr, type);
            });
        }
    }
}
