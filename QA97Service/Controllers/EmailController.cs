using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace QA97Service.Controllers
{
    public class EmailController : ApiController
    {
        public bool SendEmailConfirmation(string code, string name, string recipient)
        {
            string subject = "QA97 Email Confirmation - testing";
            var callbackUrl = "testing";
            string body = "<p>Hi  : " + name + ", <br> Please confirm your account by clicking this link: <a href=\""
                                               + code + "\">link</a>. <br></p> <br>This is a part of email API testing</i></p><br> <p>Regards,<br> <i>Shubham Saxena <br> Developer QA97<i></p>";
            return SendEmail(body, subject, recipient);
        }

        [HttpGet]
        public bool SendEmail(string message, string subject, string recipient)
        {
            try
            {
                string FromMail = "mail@shubhamsaxena.com";
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
                return true;
            }
            catch (Exception e) { return false; }
        }
    }
}
