using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using log4net.Core;
using System.Configuration;
using System.Net.Configuration;

namespace Radial.Net
{
    /// <summary>
    /// Smtp mail class
    /// </summary>
    public sealed class SmtpMail
    {
        SmtpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmtpMail"/> class by using configuration file settings.
        /// </summary>
        public SmtpMail()
        {
            _client = new SmtpClient();
        }

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
            _client = new SmtpClient(host, port);
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.EnableSsl = enableSsl;
        }


        /// <summary>
        /// Gets the sender credential.
        /// </summary>
        public NetworkCredential SenderCredential
        {
            get
            {
                return _client.Credentials as NetworkCredential;
            }
        }

        /// <summary>
        /// Gets the smtp section in configuration file.
        /// </summary>
        public SmtpSection ConfigurationSection
        {
            get
            {
                return ConfigurationManager.GetSection("system.net/mailSettings/smtp") as SmtpSection;
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private Logger Logger
        {
            get
            {
                return Radial.Logger.GetInstance("SmtpMail");
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
        /// <param name="message">The mail message.</param>
        /// <returns>
        /// An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.
        /// </returns>
        public SmtpStatusCode Send(MailMessage message)
        {
            return Send(null, null, message, false);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCertificate">The sender certificate.</param>
        /// <param name="message">The mail message.</param>
        /// <returns>
        /// An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.
        /// </returns>
        public SmtpStatusCode Send(X509Certificate senderCertificate, MailMessage message)
        {
            return Send(null, senderCertificate, message, false);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="message">The mail message.</param>
        /// <param name="async">if set to <c>true</c> will use asynchronous call and always return SmtpStatusCode.Ok.</param>
        /// <returns>
        /// An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.
        /// </returns>
        public SmtpStatusCode Send(MailMessage message, bool async)
        {
            return Send(null, null, message, async);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCertificate">The sender certificate.</param>
        /// <param name="message">The mail message.</param>
        /// <param name="async">if set to <c>true</c> will use asynchronous call and always return SmtpStatusCode.Ok.</param>
        /// <returns>
        /// An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.
        /// </returns>
        public SmtpStatusCode Send(X509Certificate senderCertificate, MailMessage message, bool async)
        {
            return Send(null, senderCertificate, message, async);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCredential">The sender credential.</param>
        /// <param name="message">The mail message.</param>
        /// <returns>An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.</returns>
        public SmtpStatusCode Send(NetworkCredential senderCredential, MailMessage message)
        {
            return Send(senderCredential, null, message, false);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCredential">The sender credential.</param>
        /// <param name="senderCertificate">The sender certificate.</param>
        /// <param name="message">The mail message.</param>
        /// <returns>An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.</returns>
        public SmtpStatusCode Send(NetworkCredential senderCredential, X509Certificate senderCertificate, MailMessage message)
        {
            return Send(senderCredential, senderCertificate, message, false);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCredential">The sender credential.</param>
        /// <param name="message">The mail message.</param>
        /// <param name="async">if set to <c>true</c> will use asynchronous call and always return SmtpStatusCode.Ok.</param>
        /// <returns>An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.</returns>
        public SmtpStatusCode Send(NetworkCredential senderCredential, MailMessage message, bool async)
        {
            return Send(senderCredential, null, message, async);
        }

        /// <summary>
        /// Sends mail message.
        /// </summary>
        /// <param name="senderCredential">The sender credential.</param>
        /// <param name="senderCertificate">The sender certificate.</param>
        /// <param name="message">The mail message.</param>
        /// <param name="async">if set to <c>true</c> will use asynchronous call and always return SmtpStatusCode.Ok.</param>
        /// <returns>An System.Net.Mail.SmtpStatusCode value that indicates the error that occurred.</returns>
        public SmtpStatusCode Send(NetworkCredential senderCredential, X509Certificate senderCertificate, MailMessage message, bool async)
        {
            Checker.Parameter(message != null, "mail message can not be null");

            SmtpStatusCode code = SmtpStatusCode.Ok;

            Checker.Requires(senderCredential != null || _client.Credentials != null, "sender credential not exist");

            if (senderCredential != null)
                _client.Credentials = senderCredential;

            if (senderCertificate != null)
                _client.ClientCertificates.Add(senderCertificate);


            try
            {
                Logger.Debug("begin send mail from {0} to {1}, async={2}", message.From.Address, BuildToAddressString(message.To), async);

                if (async)
                {
                    _client.SendCompleted += new SendCompletedEventHandler(SendCompleted);
                    _client.SendAsync(message, new { from = message.From.Address, to = BuildToAddressString(message.To) });
                }
                else
                {
                    _client.Send(message);

                    Logger.Debug("end send mail from {0} to {1}, async={2}", message.From.Address, BuildToAddressString(message.To), async);
                }
            }
            catch (SmtpException ex)
            {
                code = ex.StatusCode;
                Logger.Error(ex, "can not send mail from {0} to {1}, async={2}", message.From.Address, BuildToAddressString(message.To), async);
            }

            return code;
        }

        /// <summary>
        /// Sends the completed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.ComponentModel.AsyncCompletedEventArgs"/> instance containing the event data.</param>
        private void SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Logger.Fatal(e.Error);
                return;
            }

            if (e.Cancelled)
            {
                Logger.Debug("send mail from {0} to {1} has been cancelled, async=True", ((dynamic)e.UserState).from, ((dynamic)e.UserState).to);
                return;
            }

            Logger.Debug("end send mail from {0} to {1}, async=True", ((dynamic)e.UserState).from, ((dynamic)e.UserState).to);
        }


    }
}
