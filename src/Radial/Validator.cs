using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Radial
{
    /// <summary>
    /// The validation class.
    /// </summary>
    public static class Validator
    {
        #region Validate Email

        /// <summary>
        /// Determines whether the specified source is email.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is email; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmail(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            if (HasChinese(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified source has email.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source has email; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasEmail(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            if (HasChinese(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Validate Url

        /// <summary>
        /// Determines whether the specified source is URL.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is URL; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsUrl(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Determines whether the specified source has URL.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source has URL; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasUrl(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            return Regex.IsMatch(source.Trim(), @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Validate DateTime


        /// <summary>
        /// Determines whether [is date time] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if [is date time] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsDateTime(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            try
            {
                DateTime time = Convert.ToDateTime(source.Trim());
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Validate Mobile

        /// <summary>
        /// Determines whether the specified source is mobile.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is mobile; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMobile(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"^1[3578]\d{9}$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Determines whether the specified source has mobile.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source has mobile; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasMobile(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"1[3578]\d{9}", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Validate IP

        /// <summary>
        /// Determines whether the specified source is IP.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is IP; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIP(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            return Regex.IsMatch(source.Trim(), @"^(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])$", RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// Determines whether the specified source has IP.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source has IP; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasIP(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])", RegexOptions.IgnoreCase);
        }
        #endregion

        #region Validate ID card


        /// <summary>
        /// Determines whether [is ID card] [the specified id].
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>
        ///   <c>true</c> if [is ID card] [the specified id]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIDCard(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return false;

            Id = Id.Trim();

            if (Id.Length == 18)
            {
                bool check = IsIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = IsIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is ID card18] [the specified id].
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>
        ///   <c>true</c> if [is ID card18] [the specified id]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsIDCard18(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return false;

            Id = Id.Trim();

            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//number validation
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//province validation
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//birth validation
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//validation code
            }
            return true;//match GB11643-1999
        }

        /// <summary>
        /// Determines whether [is ID card15] [the specified id].
        /// </summary>
        /// <param name="Id">The id.</param>
        /// <returns>
        ///   <c>true</c> if [is ID card15] [the specified id]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsIDCard15(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return false;

            Id = Id.Trim();

            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//number validation
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//province validation
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//birth validation
            }
            return true;//match 15 bit id card
        }
        #endregion

        #region Is Int

        /// <summary>
        /// Determines whether the specified source is int.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is int; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInt(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            source = source.Trim();
            Regex regex = new Regex(@"^(-){0,1}\d+$");
            if (regex.Match(source).Success)
            {
                if ((long.Parse(source) > 0x7fffffffL) || (long.Parse(source) < -2147483648L))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion


        /// <summary>
        /// Determines whether the specified source is chinese tel.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if the specified source is tel; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsTel(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            return Regex.IsMatch(source.Trim(), @"^\d{3,4}-?\d{6,8}$", RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// Determines whether [is post code] [the specified source].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if [is post code] [the specified source]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPostCode(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), @"^\d{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Determines whether the specified source contains chinese character.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>
        ///   <c>true</c> if contains chinese characters; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasChinese(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            return Regex.IsMatch(source.Trim(), "[\u4e00-\u9fa5]");
        }
    }
}
