using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace QA97Service.Controllers
{
    public class EmailController : ApiController
    {
        [HttpGet]
        public async void SendEmail(string message, string subject, string recipient)
        {
            try { string FromMail = "mail@shubhamsaxena.com";
                string emailTo = recipient;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.shubhamsaxena.com");
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(FromMail);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = message;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mail@shubhamsaxena.com", "shubham.0987");
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
            }
            catch (Exception e)
            {
                string s = "shubham";
            }
        }
    }
}
