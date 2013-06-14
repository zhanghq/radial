using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using NUnit.Framework;
using Radial.Net;

namespace Radial.UnitTest
{
    [TestFixture]
    public class SmtpMailTest
    {
        [Test]
        public void Send()
        {
            SmtpMail client = new SmtpMail("smtp.163.com");
            MailMessage msg=new MailMessage();
            msg.From=new MailAddress("ihaiqing@163.com");
            msg.To.Add("me@zhanghq.net");
            msg.Subject = "Test";
            msg.Body="<p>Hello</p>";
            msg.IsBodyHtml=true;
            msg.BodyEncoding=Encoding.UTF8;
            Cycler.Execute(() => client.Send("ihaiqing@163.com", "0556016006", msg), 2, 1000);
        }
    }
}
