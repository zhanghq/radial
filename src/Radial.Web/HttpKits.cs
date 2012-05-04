using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;
using Radial.Serialization;
using System.Text.RegularExpressions;
using System.Net;
using Radial.Net;

namespace Radial.Web
{
    /// <summary>
    /// Toolkits class for http context
    /// </summary>
    public static class HttpKits
    {
        /// <summary>
        /// Plain Text Content Type
        /// </summary>
        public const string PlainTextContentType = "text/plain";

        /// <summary>
        /// Xml Content Type
        /// </summary>
        public const string XmlContentType = "text/xml";
        /// <summary>
        /// Json Content Type
        /// </summary>
        public const string JsonContentType = "application/json";

        /// <summary>
        /// Gets the current HttpContext instance.
        /// </summary>
        public static HttpContext CurrentContext
        {
            get
            {
                return HttpContext.Current;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a web application.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is a web application; otherwise, <c>false</c>.
        /// </value>
        public static bool IsWebApp
        {
            get
            {
                return !string.IsNullOrWhiteSpace(System.Web.HttpRuntime.AppDomainAppId);
            }
        }

        /// <summary>
        /// Get the cookie
        /// </summary>
        /// <param name="name">The cookie name</param>
        /// <returns>If found return the instance, otherwise return null</returns>
        public static HttpCookie GetCookie(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "cookie name can not be empty or null");

            return CurrentContext.Request.Cookies[name.Trim()];
        }

        /// <summary>
        /// Set the cookie
        /// </summary>
        /// <param name="cookie">The cookie instance</param>
        public static void SetCookie(HttpCookie cookie)
        {
            Checker.Parameter(cookie != null, "cookie instance can not be null");
            CurrentContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Remove the cookie
        /// </summary>
        /// <param name="name">The cookie name</param>
        public static void RemoveCookie(string name)
        {
            HttpCookie cookie = GetCookie(name);

            if (cookie != null)
            {
                cookie.Expires = DateTime.MinValue;
                CurrentContext.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// Set value to HttpSessionState
        /// </summary>
        /// <param name="name">The session name</param>
        /// <param name="value">The session value</param>
        public static void SetSession(string name, object value)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "session name can not be empty or null");
            CurrentContext.Session[name] = value;
        }

        /// <summary>
        /// Set value to HttpSessionState
        /// </summary>
        /// <typeparam name="T">The session value type</typeparam>
        /// <param name="name">The session name</param>
        /// <param name="value">The session value</param>
        public static void SetSession<T>(string name, T value)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "session name can not be empty or null");
            CurrentContext.Session[name] = value;
        }

        /// <summary>
        /// Get value from HttpSessionState
        /// </summary>
        /// <param name="name">The session name</param>
        /// <returns>The session value</returns>
        public static object GetSession(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "session name can not be empty or null");
            return CurrentContext.Session[name];
        }

        /// <summary>
        /// Get value from HttpSessionState
        /// </summary>
        /// <typeparam name="T">The session value type</typeparam>
        /// <param name="name">The session name</param>
        /// <returns>
        /// The session value
        /// </returns>
        public static T GetSession<T>(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "session name can not be empty or null");
            object obj = CurrentContext.Session[name];

            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// Remove session
        /// </summary>
        /// <param name="name">The session name</param>
        public static void RemoveSession(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "session name can not be empty or null");
            CurrentContext.Session.Remove(name);
        }

        /// <summary>
        /// Write the image bytes to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="imgBytes">The image bytes.</param>
        public static void WriteImage(byte[] imgBytes)
        {
            WriteImage(imgBytes, string.Empty);
        }

        /// <summary>
        /// Write the image bytes to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="imgBytes">The image bytes.</param>
        /// <param name="contentType">the content type.</param>
        public static void WriteImage(byte[] imgBytes, string contentType)
        {
            Checker.Parameter(imgBytes != null, "imgBytes can not be null");

            if (string.IsNullOrWhiteSpace(contentType))
                CurrentContext.Response.ContentType = "image/jpeg";
            else
                CurrentContext.Response.ContentType = contentType.Trim();

            CurrentContext.Response.Clear();
            CurrentContext.Response.OutputStream.Write(imgBytes, 0, imgBytes.Length);
            CurrentContext.ApplicationInstance.CompleteRequest();

        }

        /// <summary>
        /// Write the image object to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        public static void WriteImage(Image image, ImageFormat format)
        {
            WriteImage(image, format, string.Empty);
        }

        /// <summary>
        /// Write the image object to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="format">The image format.</param>
        /// <param name="contentType">the content type.</param>
        public static void WriteImage(Image image, ImageFormat format, string contentType)
        {
            Checker.Parameter(image != null, "image object can not be null");
            CurrentContext.Response.Clear();

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);

                CurrentContext.Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }
            
            if (string.IsNullOrWhiteSpace(contentType))
                CurrentContext.Response.ContentType = "image/" + format.ToString().ToLower();
            else
                CurrentContext.Response.ContentType = contentType.Trim();
            CurrentContext.ApplicationInstance.CompleteRequest();
        }


        /// <summary>
        /// Write the image stream to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="imgStream">The image stream.</param>
        public static void WriteImage(Stream imgStream)
        {
            WriteImage(imgStream, string.Empty);
        }

        /// <summary>
        /// Write the image stream to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="imgStream">The image stream.</param>
        /// <param name="contentType">the content type.</param>
        public static void WriteImage(Stream imgStream, string contentType)
        {
            Checker.Parameter(imgStream != null, "imgStream can not be null");

            List<byte> byteList = new List<byte>();

            using (imgStream)
            {
                int n = imgStream.ReadByte();
                while (n > -1)
                {
                    byteList.Add((byte)n);

                    n = imgStream.ReadByte();
                }
            }

            WriteImage(byteList.ToArray(), contentType);

        }

        /// <summary>
        /// Write the plain text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="text">The plain text</param>
        public static void WritePlainText(string text)
        {
            WritePlainText(text, string.Empty);
        }

        /// <summary>
        /// Write the plain text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="text">The plain text</param>
        /// <param name="contentType">the content type</param>
        public static void WritePlainText(string text, string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType))
                CurrentContext.Response.ContentType = PlainTextContentType;
            else
                CurrentContext.Response.ContentType = contentType.Trim();

            CurrentContext.Response.Write(text);
            CurrentContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Write the json text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="obj">The object.</param>
        public static void WriteJson<T>(T obj)
        {
            WritePlainText(JsonSerializer.Serialize<T>(obj), JsonContentType);
        }

        /// <summary>
        /// Write the json text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void WriteJson(object obj)
        {
            WritePlainText(JsonSerializer.Serialize(obj), JsonContentType);
        }

        /// <summary>
        /// Write the json text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="json">The json text</param>
        public static void WriteJson(string json)
        {
            WritePlainText(json, JsonContentType);
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="xml">The xml text</param>
        public static void WriteXml(string xml)
        {
            WriteXml(xml, "UTF-8");
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="xml">The xml text</param>
        /// <param name="charset">The xml charset</param>
        public static void WriteXml(string xml, string charset)
        {
            if (string.IsNullOrWhiteSpace(charset))
                charset = "UTF-8";

            if (string.IsNullOrWhiteSpace(xml))
                xml = string.Format("<?xml version=\"1.0\" encoding=\"{0}\"?>", charset);

            CurrentContext.Response.ContentType = XmlContentType;
            CurrentContext.Response.Charset = charset;
            CurrentContext.Response.Write(xml);
            CurrentContext.ApplicationInstance.CompleteRequest();
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="obj">The obj.</param>
        public static void WriteXml(object obj)
        {
            WriteXml(obj, "UTF-8");
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="charset">The charset.</param>
        public static void WriteXml(object obj, string charset)
        {
            WriteXml(Serialization.XmlSerializer.Serialize(obj), charset);
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="obj">The obj.</param>
        public static void WriteXml<T>(T obj)
        {
            WriteXml(obj, "UTF-8");
        }

        /// <summary>
        /// Write the xml text to response stream (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="obj">The obj.</param>
        /// <param name="charset">The charset.</param>
        public static void WriteXml<T>(T obj, string charset)
        {
            WriteXml(Serialization.XmlSerializer.Serialize<T>(obj), charset);
        }

        /// <summary>
        /// Export dataset to Excel file (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="ds">The dataset object</param>
        /// <param name="fileName">The Excel file name(not contains extension)</param>
        public static void ExportToExcel(DataSet ds, string fileName)
        {
            ExportToExcel(ds,fileName,Encoding.UTF8);
        }

        /// <summary>
        /// Export dataset to Excel file (use return statement to bypass other codes if necessary).
        /// </summary>
        /// <param name="ds">The dataset object</param>
        /// <param name="fileName">The Excel file name(not contains extension)</param>
        /// <param name="encoding">The encoding.</param>
        public static void ExportToExcel(DataSet ds, string fileName,Encoding encoding)
        {
            Checker.Parameter(ds != null, "dataset object can not be null");
            Checker.Parameter(encoding!=null, "encoding can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(fileName), "fileName can not be empty or null");

            StringBuilder strb = new StringBuilder();

            strb.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\"/>");
            strb.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html\" charset=\"" + encoding.BodyName + "\"/>");
            strb.Append("<xml>");
            strb.Append("<x:ExcelWorkbook>");
            strb.Append("<x:ExcelWorksheets>");
            strb.Append("<x:ExcelWorksheet>");
            strb.Append("<x:Name>Sheet1</x:Name>");
            strb.Append("<x:WorksheetOptions>");
            strb.Append("<x:DefaultRowHeight>285</x:DefaultRowHeight>");
            strb.Append("<x:Selected/>");
            strb.Append("<x:Panes>");
            strb.Append("<x:Pane>");
            strb.Append("<x:Number>3</x:Number>");
            strb.Append("<x:ActiveCol>1</x:ActiveCol>");
            strb.Append("</x:Pane>");
            strb.Append("</x:Panes>");
            strb.Append("/x:WorksheetOptions>");
            strb.Append("</x:ExcelWorksheet>");
            strb.Append("</x:ExcelWorksheets>");
            strb.Append("</x:ExcelWorkbook>");
            strb.Append("</xml>");
            strb.Append("</head><body><table align=\"center\" style='border-collapse:collapse;table-layout:fixed'><tr>");

            if (ds.Tables.Count > 0)
            {
                //title
                int columncount = ds.Tables[0].Columns.Count;
                for (int columi = 0; columi < columncount; columi++)
                {
                    strb.Append("<td>" + ds.Tables[0].Columns[columi] + "</td>");
                }
                strb.Append("</tr>");

                //data    
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strb.Append("<tr>");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        strb.Append("<td>" + ds.Tables[0].Rows[i][j].ToString() + "</td>");
                    }
                    strb.Append("</tr>");
                }
            }

            strb.Append("</body></html>");

            CurrentContext.Response.Clear();
            CurrentContext.Response.Buffer = true;
            CurrentContext.Response.Charset = encoding.BodyName;
            CurrentContext.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, encoding)+".xls");
            CurrentContext.Response.ContentEncoding = encoding;
            CurrentContext.Response.ContentType = "application/vnd.ms-excel";
            CurrentContext.Response.Write(strb);
            CurrentContext.Response.End();

        }

        /// <summary>
        /// Resolves the string to parameters.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        /// Parameters collection, if parameter string is empty or null return an empty collection.
        /// </returns>
        public static NameValueCollection ResolveParameters(Uri uri)
        {
            Checker.Parameter(uri != null, "uri can not be null");

            return ResolveParameters(uri.Query);
        }

        /// <summary>
        /// Resolves the string to parameters.
        /// </summary>
        /// <param name="paramString">The parameter string with &amp; separator.</param>
        /// <returns>Parameters collection, if parameter string is empty or null return an empty collection.</returns>
        public static NameValueCollection ResolveParameters(string paramString)
        {
            NameValueCollection collection = new NameValueCollection();

            if (!string.IsNullOrWhiteSpace(paramString))
            {
                paramString = paramString.Trim('?', '&');
                string[] split = paramString.Split('&');

                foreach (string s in split)
                {
                    string[] s2 = s.Split('=');

                    if (s2.Length > 1)
                    {
                        string key = s2[0].Trim().ToLower();
                        string value = s2.Length == 2 ? System.Web.HttpUtility.UrlDecode(s2[1]) : string.Empty;

                        collection.Add(key, value);
                    }
                }
            }
            return collection;
        }

        /// <summary>
        /// Get the file extension corresponding Content-Type
        /// </summary>
        /// <param name="fileExtension">The file extension(with dot symbol)</param>
        /// <returns>If found return the Content-Type string, otherwise return "application/octet-stream" as default</returns>
        public static string GetContentType(string fileExtension)
        {
            string contentType = "application/octet-stream";

            if (!string.IsNullOrWhiteSpace(fileExtension))
            {
                Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(fileExtension);

                if (registryKey != null && registryKey.GetValue("Content Type") != null)
                    contentType = registryKey.GetValue("Content Type").ToString();
            }

            return contentType;
        }

        /// <summary>
        /// Makes the relative url to absolute url.
        /// </summary>
        /// <param name="relativeUrl">The relative url.</param>
        /// <returns>The absolute url.</returns>
        public static string MakeAbsoluteUrl(string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(relativeUrl))
                relativeUrl = "";
            else
                relativeUrl=relativeUrl.Trim('~', '/');

            if (relativeUrl.ToLower().StartsWith("http://") || relativeUrl.ToLower().StartsWith("https://") || relativeUrl.ToLower().StartsWith("ftp://"))
                return relativeUrl;

            string url = string.Format("{0}://{1}", CurrentContext.Request.Url.Scheme, CurrentContext.Request.Url.Host);

            if (!((CurrentContext.Request.Url.Scheme == "http" && CurrentContext.Request.Url.Port == 80) || (CurrentContext.Request.Url.Scheme == "https" && CurrentContext.Request.Url.Port == 443)))
            {
                url += ":" + CurrentContext.Request.Url.Port;
            }

            string path = (CurrentContext.Request.ApplicationPath.Trim('/') + "/" + relativeUrl).Trim('/');

            if (string.IsNullOrWhiteSpace(path))
                return url;
            else
                return url + "/" + path;
        }

        /// <summary>
        /// Makes the absolute url to relative url.
        /// </summary>
        /// <param name="absoluteUrl">The absolute url.</param>
        /// <returns>The relative url.</returns>
        public static string MakeRelativeUrl(string absoluteUrl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(absoluteUrl), "absoluteUrl can not be empty or null");

            absoluteUrl=absoluteUrl.Trim();

            string url = string.Format("{0}://{1}", CurrentContext.Request.Url.Scheme, CurrentContext.Request.Url.Host);

            if (!((CurrentContext.Request.Url.Scheme == "http" && CurrentContext.Request.Url.Port == 80) || (CurrentContext.Request.Url.Scheme == "https" && CurrentContext.Request.Url.Port == 443)))
            {
                url += ":" + CurrentContext.Request.Url.Port;
            }

            url += "/" + CurrentContext.Request.ApplicationPath.Trim('/');
            url = url.Trim('/');

            string temp = absoluteUrl.Replace(url, "");

            if (!temp.StartsWith("/"))
                return "~/" + temp;
            else
                return "~" + temp;
        }

        /// <summary>
        /// Javascript Alert.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="message">The message.</param>
        public static void Alert(Page page, string message)
        {
            Alert(page, message, string.Empty);
        }

        /// <summary>
        /// Javascript Alert.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="message">The message.</param>
        /// <param name="redirectUrl">The redirect url.</param>
        public static void Alert(Page page, string message, string redirectUrl)
        {
            Checker.Parameter(page != null, "System.Web.UI.Page object can not be null");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("alert(\"" + message + "\");");

            if (!string.IsNullOrWhiteSpace(redirectUrl))
                sb.AppendLine("window.location=\"" + redirectUrl + "\";");

            RegisterScript(page, sb.ToString());
        }


        /// <summary>
        /// Registers javascript to System.Web.UI.Page
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="script">The script.</param>
        public static void RegisterScript(Page page, string script)
        {
            Checker.Parameter(page != null, "System.Web.UI.Page object can not be null");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<script type=\"text/javascript\">");

            sb.Append(script);

            sb.AppendLine("</script>");

            page.ClientScript.RegisterStartupScript(page.GetType(), "", sb.ToString());
        }

        /// <summary>
        /// Strip html code from a hypertext string.
        /// </summary>
        /// <param name="strHtml">The hypertext string</param>
        /// <returns>Plain text string</returns>
        public static string StripHtml(string strHtml)
        {
            string[] aryReg ={
          @"<script[^>]*?>.*?</script>",
          @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
          @"([\r\n])[\s]+",
          @"&(quot|#34);",
          @"&(amp|#38);",
          @"&(lt|#60);",
          @"&(gt|#62);", 
          @"&(nbsp|#160);", 
          @"&(iexcl|#161);",
          @"&(cent|#162);",
          @"&(pound|#163);",
          @"&(copy|#169);",
          @"&#(\d+);",
          @"-->",
          @"<!--.*\n"
         };
            string[] aryRep = {
           "",
           "",
           "",
           "\"",
           "&",
           "<",
           ">",
           " ",
           "\xa1",//chr(161),
           "\xa2",//chr(162),
           "\xa3",//chr(163),
           "\xa9",//chr(169),
           "",
           "\r\n",
           ""
          };
            string newReg = aryReg[0];

            string strOutput = strHtml;

            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, aryRep[i]);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");
            return strOutput;
        }


        /// <summary>
        /// Converts a url string to its escaped representation.
        /// </summary>
        /// <param name="url">The input url.</param>
        /// <returns>A System.String that contains the escaped representation of input url.</returns>
        public static string EscapeUrl(string url)
        {
            return Uri.EscapeDataString(url).Replace("!", "%21").Replace("*", "%2A").Replace("(", "%28").Replace(")", "%29").Replace("'", "%27");
        }

        /// <summary>
        /// Converts a url string to its unescaped representation.
        /// </summary>
        /// <param name="url">The input url.</param>
        /// <returns>A System.String that contains the unescaped representation of input url.</returns>
        public static string UnescapeUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
                url = url.Replace("%21", "!").Replace("%2A", "*").Replace("%28", "(").Replace("%29", ")").Replace("%27", "'");
            return Uri.UnescapeDataString(url);
        }


        /// <summary>
        /// Gets the location based on ip address.
        /// </summary>
        /// <param name="ipAddress">The ip address.</param>
        /// <returns>If no location matched return string.Empty, otherwise return the location based on ip address.</returns>
        public static string GetLocation(string ipAddress)
        {
            Checker.Parameter(Validator.IsIP(ipAddress), "ip address format error:{0}", ipAddress);

            string serverUrl = string.Format("http://opendata.baidu.com/api.php?query={0}&resource_id=6006&format=json", ipAddress.Trim());

            HttpResponseObj resp= HttpWebHost.Get(serverUrl);

            if (resp.Code == System.Net.HttpStatusCode.OK)
            {
                dynamic obj = Serialization.JsonSerializer.Deserialize<dynamic>(resp.Text);

                if (obj != null && obj.data != null && obj.data.Count == 1)
                {
                    if (obj.data[0].location != null)
                        return ((string)obj.data[0].location).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                }
                return string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the client IPv4 address.
        /// </summary>
        /// <returns>If no error occurs return client IPv4 address, otherwise return string.Empty.</returns>
        public static string GetClientIPv4Address()
        {
            string userHostAddress = CurrentContext.Request.UserHostAddress;

            if (!string.IsNullOrWhiteSpace(CurrentContext.Request.ServerVariables["HTTP_VIA"]))
                userHostAddress = CurrentContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            string ipv4 = string.Empty;

            if (!string.IsNullOrWhiteSpace(userHostAddress))
            {
                //get first one
                userHostAddress = userHostAddress.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];

                IPAddress userHostIpAddressObj = null;
                if (IPAddress.TryParse(userHostAddress, out userHostIpAddressObj))
                {
                    foreach (IPAddress ipa in Dns.GetHostEntry(userHostIpAddressObj).AddressList)
                    {
                        if (ipa.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            ipv4 = ipa.ToString();
                            break;
                        }
                    }
                }
            }

            return ipv4;
        }
    }
}
