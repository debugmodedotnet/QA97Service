using Newtonsoft.Json;
using QA97Service.Entities;
using QA97Service.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace QA97Service.Controllers
{
    public class SMSController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public async void post()
        {
            var requestContent = Request.Content.ReadAsFormDataAsync().Result;
            string content = requestContent.GetValues("content").FirstOrDefault();
            string sender = requestContent.GetValues("sender").FirstOrDefault();
            string title = requestContent.GetValues("comments").FirstOrDefault();

            string password="";
            string UserId = (from r in db.Users where r.PhoneNumber == sender select r.UserName).FirstOrDefault();
            if (UserId == null)
            {                
                AccountController ac = new AccountController();
                ac.ControllerContext = this.ControllerContext;
                RegisterBindingModel userData = new RegisterBindingModel();
                password = Membership.GeneratePassword(8, 1);
                 
                userData.Password = password;
                userData.ConfirmPassword = password;
                userData.FullName = "SMS User";
                userData.PhoneNumber = sender;
                UserId = userData.Email = sender + "@qa97.com";

                IHttpActionResult x = await ac.Register(userData);
            }

            QuestionsController q = new QuestionsController();
            Question question = new Question();
            question.UserId = UserId;
            question.QuestionTitle = title;
            question.QuestionDetailRichText = content;
            question.QuestionDetailPlainText = content;
            //values for SMS category
            question.SubjectId = 1;
            question.ClassId = 1;

            q.PostQuestion(question);
        }

        [HttpGet]
        public string SendSMS(string phoneNumber, string message)
        {
                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("http://api.textlocal.in/send/",
                        new NameValueCollection()
                {
                {"username" , "shubham0987@gmail.com"},
                {"hash" , "a26e5ce77dce49c36e0a58530058d9d1e793cb7b"},
                {"numbers" , phoneNumber},
                {"message" , message},
                {"sender" , "TXTLCL"}
                });
                    return System.Text.Encoding.UTF8.GetString(response);
                }
        }
    }
}
