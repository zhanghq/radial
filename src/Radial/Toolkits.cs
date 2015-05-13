using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Net.NetworkInformation;
using System.Threading;
using Radial.Net;
using Radial.Extensions;

namespace Radial
{
    /// <summary>
    /// Toolkits class.
    /// </summary>
    public static class Toolkits
    {
        /// <summary>
        /// Gets the enumeration item.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="enumValue">The enumeration value.</param>
        /// <returns>The enumeration item, if not match throw a NotSupportedException.</returns>
        public static TEnum GetEnumItem<TEnum>(int? enumValue) where TEnum : struct
        {
            Checker.Parameter(typeof(TEnum).IsEnum, "TEnum must be an enumeration");

            if (enumValue == null)
                return default(TEnum);

            FieldInfo[] infos = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);


            foreach (FieldInfo f in infos)
            {
                int fv = (int)f.GetValue(null);
                if (fv == enumValue)
                {
                    return (TEnum)f.GetValue(null);
                }
            }

            throw new NotSupportedException(string.Format("{0} does not support value {1}", typeof(TEnum).FullName, enumValue));
        }

        /// <summary>
        /// Gets the enumeration item description.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="enumValue">The enumeration value.</param>
        /// <returns>
        /// The item description, if not find EnumItemAttribute return String.Empty.
        /// </returns>
        public static string GetEnumItemDescription<TEnum>(TEnum? enumValue) where TEnum : struct
        {
            Checker.Parameter(typeof(TEnum).IsEnum, "TEnum must be an enumeration");

            if (enumValue == null)
                return string.Empty;

            FieldInfo[] infos = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);


            foreach (FieldInfo f in infos)
            {
                TEnum fv = (TEnum)f.GetValue(null);
                if (fv.Equals(enumValue))
                {
                    object[] attObjs = f.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                    string desp = string.Empty;
                    if (attObjs.Length == 1)
                        desp = (attObjs[0] as System.ComponentModel.DescriptionAttribute).Description;

                    return desp;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the enum value-description pairs.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <returns>
        /// Return enum value-description pairs, key=enum value, value=enum description.
        /// </returns>
        public static KeyValuePair<int, string>[] GetEnumValueDescriptions<TEnum>() where TEnum : struct
        {
            Checker.Parameter(typeof(TEnum).IsEnum, "TEnum must be an enumeration");

            FieldInfo[] infos = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public);

            KeyValuePair<int, string>[] pairs = new KeyValuePair<int, string>[infos.Length];

            for (int i = 0; i < infos.Length; i++)
            {
                int val = (int)infos[i].GetValue(null);

                string desp = null;

                object[] attObjs = infos[i].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attObjs.Length == 1)
                    desp = (attObjs[0] as System.ComponentModel.DescriptionAttribute).Description;

                pairs[i] = new KeyValuePair<int, string>(val, desp);
            }

            return pairs;
        }


        /// <summary>
        /// Convert ip string to int array.
        /// </summary>
        /// <param name="ip">The ip string.</param>
        /// <returns>int array of ip string.</returns>
        private static int[] ToIntArray(string ip)
        {
            Checker.Requires(Validator.IsIP(ip), "ip format is incorrect:{0}.", ip);

            ip = ip.Trim();

            int[] retip = new int[4];
            int i, count;
            char c;
            for (i = 0, count = 0; i < ip.Length; i++)
            {
                c = ip[i];
                if (c != '.')
                    retip[count] = retip[count] * 10 + int.Parse(c.ToString());
                else
                    count++;
            }
            return retip;
        }

        /// <summary>
        /// Determines whether the ip is in range.
        /// </summary>
        /// <param name="ip">The ip string</param>
        /// <param name="beginIp">The range starting ip</param>
        /// <param name="endIp">The range ending ip</param>
        /// <returns><c>true</c> if the input ip string is in range ; otherwise, <c>false</c>.</returns>
        public static bool IsIncludedInScope(string ip, string beginIp, string endIp)
        {
            Checker.Requires(Validator.IsIP(ip), "ip format is incorrect:{0}.", ip);
            Checker.Requires(Validator.IsIP(beginIp), "beginIp format is incorrect:{0}.", beginIp);
            Checker.Requires(Validator.IsIP(endIp), "endIp format is incorrect:{0}.", endIp);

            ip = ip.Trim();
            beginIp = beginIp.Trim();
            endIp = endIp.Trim();

            if (beginIp == endIp && ip == beginIp)
                return true;

            int[] inip, begipint, endipint = new int[4];
            inip = ToIntArray(ip);
            begipint = ToIntArray(beginIp);
            endipint = ToIntArray(endIp);
            for (int i = 0; i < 4; i++)
            {
                if (inip[i] < begipint[i] || inip[i] > endipint[i])
                {
                    return false;
                }
                else if (inip[i] > begipint[i] || inip[i] < endipint[i])
                {
                    return true;
                }
            }
            return true;
        }

        /// <summary>
        /// Upper case the first char of the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>new string.</returns>
        public static string FirstCharUpperCase(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                return input[0].ToString().ToUpper() + (input.Length > 1 ? input.Substring(1) : string.Empty);
            return input;
        }


        /// <summary>
        /// Compress
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>
        /// Compressed string.
        /// </returns>
        public static string Compress(string str)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(str), "input string can not be empty or null.");
            byte[] cb = Compress(StaticVariables.Encoding.GetBytes(str));

            StringBuilder ret = new StringBuilder();
            foreach (byte b in cb)
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// Compress
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>Compressed data.</returns>
        public static byte[] Compress(byte[] data)
        {
            Checker.Parameter(data != null, "input data can not be null.");

            MemoryStream ms = new MemoryStream();

            GZipStream zipStream = new GZipStream(ms, CompressionMode.Compress, true);
            zipStream.Write(data, 0, data.Length);
            zipStream.Close(); //must close before position

            List<byte> bufferList = new List<byte>();
            ms.Position = 0;
            while (true)
            {
                int b = ms.ReadByte();
                if (b < 0)
                    break;
                bufferList.Add(Convert.ToByte(b));
            }

            ms.Close();

            return bufferList.ToArray();
        }

        /// <summary>
        /// Decompress.
        /// </summary>
        /// <param name="str">The input string.</param>
        /// <returns>
        /// Decompressed string.
        /// </returns>
        public static string Decompress(string str)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(str), "input string can not be empty or null.");

            byte[] inputByteArray = new byte[str.Length / 2];
            for (int x = 0; x < inputByteArray.Length; x++)
            {
                inputByteArray[x] = Convert.ToByte(str.Substring(x * 2, 2), 16);
            }

            byte[] cb = Decompress(inputByteArray);

            return StaticVariables.Encoding.GetString(cb);
        }

        /// <summary>
        /// Decompress.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>Decompressed data.</returns>
        public static byte[] Decompress(byte[] data)
        {
            Checker.Parameter(data != null, "input data can not be null.");

            MemoryStream ms = new MemoryStream(data);

            GZipStream zipStream = new GZipStream(ms, CompressionMode.Decompress);

            List<byte> bufferList = new List<byte>();

            while (true)
            {
                int b = zipStream.ReadByte();
                if (b < 0)
                    break;
                bufferList.Add(Convert.ToByte(b));
            }

            ms.Close();

            zipStream.Close();

            return bufferList.ToArray();
        }

        /// <summary>
        /// Convert to Base64 string.
        /// </summary>
        /// <param name="data">The input string.</param>
        /// <returns>
        /// Base64 string.
        /// </returns>
        public static string ToBase64String(string data)
        {

            return ToBase64String(StaticVariables.Encoding.GetBytes(data));
        }

        /// <summary>
        /// Convert to Base64 string.
        /// </summary>
        /// <param name="data">The input data.</param>
        /// <returns>Base64 string.</returns>
        public static string ToBase64String(byte[] data)
        {
            return Convert.ToBase64String(data);
        }


        /// <summary>
        /// Convert to string from Base64 format.
        /// </summary>
        /// <param name="base64Str">The input string.</param>
        /// <returns>
        /// Decoded string.
        /// </returns>
        public static string FromBase64String(string base64Str)
        {

            return StaticVariables.Encoding.GetString(FromBase64StringToBytes(base64Str));
        }

        /// <summary>
        /// Convert to byte array from Base64 format.
        /// </summary>
        /// <param name="base64Str">The input data.</param>
        /// <returns>Decoded binary array.</returns>
        public static byte[] FromBase64StringToBytes(string base64Str)
        {
            return Convert.FromBase64String(base64Str);
        }


        /// <summary>
        /// Gets the ping replies.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="count">The ping count.</param>
        /// <param name="interval">The ping interval milliseconds.</param>
        /// <param name="bytes">The buffer bytes.</param>
        /// <param name="timeout">The timeout milliseconds.</param>
        /// <param name="ttl">The TTL.</param>
        /// <param name="dontFragment">if set to <c>true</c> [dont fragment].</param>
        /// <returns></returns>
        public static IList<PingReply> GetPingReplies(string host, int count = 4, int interval = 1000, 
            int bytes = 32, int timeout = 1000, int ttl = 128, bool dontFragment = false)
        {
            IList<PingReply> list = new List<PingReply>();

            // Create a buffer of 32 bytes of data to be transmitted.
            byte[] buffer = new byte[bytes];
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;

            using (Ping pingSender = new Ping())
            {
                for (int i = 0; i < count; i++)
                {
                    list.Add(pingSender.Send(host, timeout, buffer, new PingOptions(ttl, dontFragment)));
                    Thread.Sleep(interval);
                }
            }

            return list;
        }

        /// <summary>
        /// Convert Unix time stamp to local time..
        /// </summary>
        /// <param name="timeStamp">The Unix time stamp.</param>
        /// <returns>
        /// Local System.DateTime struct.
        /// </returns>
        public static DateTime FromUnixTimeStamp(long timeStamp)
        {
            return FromUnixTimeStamp(timeStamp, null);
        }

        /// <summary>
        /// Convert Unix time stamp to local time.
        /// </summary>
        /// <param name="timeStamp">The Unix time stamp.</param>
        /// <param name="localTimeZoneInfo">The TimeZoneInfo object of the local time.</param>
        /// <returns>
        /// Local System.DateTime struct.
        /// </returns>
        public static DateTime FromUnixTimeStamp(long timeStamp, TimeZoneInfo localTimeZoneInfo)
        {
            if (localTimeZoneInfo == null)
                localTimeZoneInfo = TimeZoneInfo.Local;
            DateTime localStartTime = TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1), localTimeZoneInfo);
            return localStartTime.AddSeconds(timeStamp);
        }


        /// <summary>
        /// Convert local time to Unix time stamp.
        /// </summary>
        /// <param name="time">The input local time.</param>
        /// <returns>
        /// The Unix time stamp.
        /// </returns>
        public static long ToUnixTimeStamp(DateTime time)
        {
            return ToUnixTimeStamp(time, null);
        }

        /// <summary>
        /// Convert local time to Unix time stamp.
        /// </summary>
        /// <param name="time">The input local time.</param>
        /// <param name="localTimeZoneInfo">The TimeZoneInfo object of the input local time.</param>
        /// <returns>
        /// The Unix time stamp.
        /// </returns>
        public static long ToUnixTimeStamp(DateTime time, TimeZoneInfo localTimeZoneInfo)
        {
            if (localTimeZoneInfo == null)
                localTimeZoneInfo = TimeZoneInfo.Local;
            DateTime localStartTime = TimeZoneInfo.ConvertTimeFromUtc(new System.DateTime(1970, 1, 1), localTimeZoneInfo);
            return (long)(time - localStartTime).TotalSeconds;

        }

        /// <summary>
        /// Deeps clone.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static object DeepClone(object obj)
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, 0);
                return bf.Deserialize(ms);
            }
        }

        /// <summary>
        /// Deeps clone.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static T DeepClone<T>(T obj) where T : class
        {
            if (obj == null)
                return null;

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Seek(0, 0);
                return bf.Deserialize(ms) as T;
            }
        }

        /// <summary>
        /// Try convert the object to its string value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="str">The string value.</param>
        /// <returns>If obj is null or can not convert return false, otherwirse return its string value.</returns>
        public static bool TryConvertToString(object obj, out string str)
        {
            str = null;

            if (obj == null)
                return false;

            Type objType = obj.GetType();

            if (objType == typeof(string))
            {
                str = obj.ToString();

                return true;
            }

            if (objType.IsEnum)
            {
                str = ((int)obj).ToString();
                return true;
            }

            try
            {
                str = Convert.ChangeType(obj, typeof(string)).ToString();
                return true;
            }
            catch { }

            return false;
        }

        /// <summary>
        /// XOR.
        /// </summary>
        /// <param name="a">The input bytes a.</param>
        /// <param name="b">The input bytes b.</param>
        /// <returns>The XOR value.</returns>
        public static byte[] Xor(byte[] a, byte[] b)
        {
            Checker.Parameter(a != null, "input bytes a can not be null.");
            Checker.Parameter(b != null, "input bytes b can not be null.");

            if (b.Length < a.Length)
            {
                byte[] temp = new byte[a.Length];
                for (int i = 0; i < temp.Length; i++)
                {
                    if (i < b.Length)
                        temp[i] = b[i];
                }

                b = temp;
            }

            byte[] c = new byte[a.Length];

            for (int i = 0; i < c.Length; i++)
            {
                c[i] = (byte)(a[i] ^ b[i]);
            }

            return c;
        }

        /// <summary>
        /// Gets the Geo information based on ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>If no location matched return null, otherwise return the Geo information based on ip address.</returns>
        public static GeoInfo GetGeoInfo(string ipAddress)
        {
            Checker.Parameter(Validator.IsIP(ipAddress), "ip address format error: {0}", ipAddress);

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
                    Logger.Default.Warn(ex, "GetGeoInfo response: {0}", resp.Text);
                }
            }

            return geo;
        }


        /// <summary>
        /// Determines whether the specified file is image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static bool IsImage(string fileName)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "file name can not be null.");

            return StaticVariables.ImageFileExtensions.Contains(o => o == Path.GetExtension(fileName).ToLower());
        }
    }
}
