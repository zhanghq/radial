using Radial.Net;
using System;

namespace Radial
{
    /// <summary>
    /// Geo information.
    /// </summary>
    public class GeoInfo
    {
        /// <summary>
        /// Gets or sets the country .
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public float? Longitude { get; set; }
        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public float? Latitude { get; set; }
        /// <summary>
        /// Gets or sets the altitude.
        /// </summary>
        public float? Altitude { get; set; }


        /// <summary>
        /// Gets the Geo information based on ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>If no location matched return null, otherwise return the Geo information based on ip address.</returns>
        public static GeoInfo Get(string ipAddress)
        {
            Checker.Parameter(Validator.IsIPv4(ipAddress), "ip address format error: {0}", ipAddress);

            string serverUrl = string.Format("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip={0}", ipAddress.Trim());

            GeoInfo geo = null;

            HttpResponseObj resp = HttpWebClient.Get(serverUrl);

            if (resp.Code == System.Net.HttpStatusCode.OK)
            {
                try
                {
                    dynamic obj = Serialization.JsonSerializer.Deserialize<dynamic>(resp.Text);

                    if (obj != null && obj.ret != null && obj.ret == 1)
                    {
                        geo = new GeoInfo();
                        if (obj.country != null)
                            geo.Country = obj.country;
                        if (obj.province != null)
                            geo.Division = obj.province;
                        if (obj.city != null)
                            geo.City = obj.city;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Get<GeoInfo>().Warn(ex, "remote response: {0}", resp.Text);
                }
            }

            return geo;
        }
    }
}
