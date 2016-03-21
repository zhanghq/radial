using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Configuration;
using System.Net.Configuration;


namespace Radial.Net
{
    /// <summary>
    /// Smtp mail class
    /// </summary>
    public sealed class SmtpMail
    {
        string _host;
        int _port = 25;
        bool _enableSsl = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMail"/> class.
        /// </summary>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        public SmtpMail(string host)
            : this(host, 25)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMail"/> class.
        /// </summary>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        public SmtpMail(string host, int port)
            : this(host, port, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMail"/> class.
        /// </summary>
        /// <param name="host">A System.String that contains the name or IP address of the host used for SMTP transactions.</param>
        /// <param name="port">An System.Int32 greater than zero that contains the port to be used on host.</param>
        /// <param name="enableSsl">if set to <c>true</c> [enable SSL].</param>
        public SmtpMail(string host, int port, bool enableSsl)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(host), "smtp host can not be empty or null");

            _host = host;
            _port = port;
            _enableSsl = enableSsl;
        }

        /// <summary>
        /// Create SmtpMail instance from the configuration file.
        /// </summary>
        /// <returns></returns>
        public static SmtpMail FromConfiguration()
        {
            return new SmtpMail(ConfigurationSection.Network.Host, ConfigurationSection.Network.Port, ConfigurationSection.Network.EnableSsl);
        }

        /// <summary>
        /// Gets the smtp section in the configuration file.
        /// </summary>
        public static SmtpSection ConfigurationSection
        {
            get
            {
                return ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            }
        }

        /// <summary>
        /// Gets the smtp account name from the configuration file.
        /// </summary>
        public static string ConfigurationAccountName
        {
            get
            {
                return ConfigurationSection.Network.UserName;
            }
        }

        /// <summary>
        /// Gets the smtp account password from the configuration file.
        /// </summary>
        public static string ConfigurationAccountPassword
        {
            get
            {
                return ConfigurationSection.Network.Password;
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private LogWriter Log
        {
            get
            {
                return Logger.New("SmtpMail");
            }
        }

        /// <summary>
        /// Builds to address string.
        /// </summary>
        /// <param name="to">To.</param>
        /// <returns></returns>
        private string BuildToAddressString(MailAddressCollection to)
        {
            List<string> tos = new List<string>(to.Count);

            if (to != null)
            {
                foreach (MailAddress a in to)
                {
                    tos.Add(a.Address);
                }
            }

            return string.Join(";", tos.ToArray());
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="smtpAccountName">Name of the SMTP account.</param>
        /// <param name="smtpAccountPassword">The SMTP account password.</param>
        /// <param name="message">The mail message.</param>
        /// <returns>If mail send ok return true, otherwise false</returns>
        public bool Send(string smtpAccountName, string smtpAccountPassword, MailMessage message)
        {
            return Send(smtpAccountName, smtpAccountPassword, null, message);
        }


        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="smtpAccountName">Name of the SMTP account.</param>
        /// <param name="smtpAccountPassword">The SMTP account password.</param>
        /// <param name="clientCertificates">The client certificates.</param>
        /// <param name="message">The mail message.</param>
        /// <returns>If mail send ok return true, otherwise false</returns>
        public bool Send(string smtpAccountName, string smtpAccountPassword, X509CertificateCollection clientCertificates, MailMessage message)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(smtpAccountName), "smtp account name can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(smtpAccountPassword), "smtp account password can not be empty or null");
            Checker.Parameter(message != null, "mail message can not be null");

            bool isOk = false;

            string tranId = Guid.NewGuid().ToString("N");

            SmtpClient client = new SmtpClient(_host, _port);
            client.EnableSsl = _enableSsl;

            client.Credentials = new NetworkCredential(smtpAccountName, smtpAccountPassword);

            if (clientCertificates != null)
            {
                foreach (var cc in clientCertificates)
                    client.ClientCertificates.Add(cc);
            }

            try
            {
                Log.Info("begin mail transaction {0}", tranId);

                Log.Debug("mail transaction {0} details: from {1} to {2}", tranId, message.From.Address, BuildToAddressString(message.To));

                client.Send(message);

                isOk = true;

                Log.Info("end mail transaction {0}", tranId);
            }
            catch (SmtpException ex)
            {
                Log.Error(ex, "can not finish mail transaction {0}", tranId);
            }

            return isOk;
        }
    }
}
