using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net.Mail;
using Radial.Net;

namespace Radial.UnitTest
{
    [TestFixture]
    public class SmtpMailTest
    {
        [Test]
        public void Send()
        {
            SmtpMail mail = new SmtpMail();
            MailMessage message = new MailMessage();
            message.From = new MailAddress(mail.ConfigurationSection.From);
            message.To.Add(new MailAddress("me@zhanghq.net"));
            bool async = true;
            message.Subject = "Greetings" + async;
            message.Body = "Greetings";

            Assert.AreEqual(SmtpStatusCode.Ok, mail.Send(message, async));

            
        }
    }
}
