using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using QA97Service.Entities;
using QA97Service.Models;
using QA97Service.ViewModel;
using System.Collections.Specialized;
using System.Xml;
using System.Web;

namespace QA97Service.Controllers
{
    public class AnswersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Answers
        public IQueryable<Answer> GetAnswers()
        {
            return db.Answers;
        }

        // GET: api/Answers/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult GetAnswer(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            return Ok(answer);
        }

        // PUT: api/Answers/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public IHttpActionResult PutAnswer(int id, Answer answer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != answer.Id)
            {
                return BadRequest();
            }

            db.Entry(answer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Answers
        [ResponseType(typeof(AnswerViewModel))]
        public IHttpActionResult PostAnswer(Answer answer)
        {
            string idOfUserAnswering = (from r in db.Users where r.UserName == answer.UserId select r.Id).FirstOrDefault();
            answer.CreatedDate = DateTime.Now;
            answer.ModifiedBy = "Foo";
            answer.ModifiedDate = DateTime.Now;
            answer.CreatedBy = "Foo";
            answer.UserId = idOfUserAnswering;
            Answer a= db.Answers.Add(answer);

            //Adding Points to User
            //UserPoint pointToUser = new UserPoint();
            //pointToUser.CreatedBy = "Foo";
            //pointToUser.ModifiedBy = "Foo";
            //pointToUser.CreatedDate = DateTime.Now;
            //pointToUser.ModifiedDate = DateTime.Now;

            //var userInPointTable = (from r in db.UserPoints where r.UserId == idOfUserAnswering select r).FirstOrDefault();
            //if (userInPointTable == null)
            //{
            //    pointToUser.UserId = idOfUserAnswering;
            //    pointToUser.Points = 5;
            //    db.UserPoints.Add(pointToUser);
            //}
            //else
            //{
            //    userInPointTable.Points = userInPointTable.Points + 5;
            //    db.Entry(userInPointTable).State = EntityState.Modified;
            //}
            //Saving to database
            db.SaveChanges();

            // creating answer view model from answer 

            //AnswerViewModel ansvm = new AnswerViewModel();

            //ansvm.AnswerDetailPlainText = a.AnswerDetailPlainText;
            //ansvm.AnswerDetailRichText = a.AnswerDetailRichText;
            //ansvm.CreatedDate = a.CreatedDate;
            //ansvm.Id = a.Id;
            //ansvm.QuestionId = a.QuestionId;
            //ansvm.Score = 10;
            //ansvm.UserId = a.UserId;
            //ansvm.UserName = a.User.FullName;

            //send SMS update an answer is posted
            string questionAskedBy = (from r in db.Questions where r.Id == answer.QuestionId select r.UserId).FirstOrDefault();
            string questionTitle = (from r in db.Questions where r.Id == answer.QuestionId select r.QuestionTitle).FirstOrDefault();
            string phoneNumber = (from r in db.Users where r.Id == questionAskedBy select r.PhoneNumber).FirstOrDefault();
            if(phoneNumber != null)
            {
                string url = "http://qa97app.azurewebsites.net/client/#/questiondetails/" + answer.QuestionId;
                XmlDocument xDoc =  ShortenURL(url);
                if (xDoc.GetElementsByTagName("status_code")[0].InnerText == "200")
                    url = xDoc.GetElementsByTagName("url")[0].InnerText;
                string msg;
                if (answer.AnswerDetailPlainText.Length < 100)
                    msg = "New answer on Q#" + answer.QuestionId + ". Answer : " + answer.AnswerDetailPlainText + ".For more details visit " + url;
                else
                    msg = "New answer on Q#" + answer.QuestionId + ".For more details visit " + url;

                SMSController sms = new SMSController();
                string SMSresponse = sms.SendSMS(phoneNumber, msg);
            }


            return CreatedAtRoute("DefaultApi", new { id = answer.Id }, answer);
        }

        // DELETE: api/Answers/5
        [ResponseType(typeof(Answer))]
        public IHttpActionResult DeleteAnswer(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return NotFound();
            }

            db.Answers.Remove(answer);
            db.SaveChanges();

            return Ok(answer);
        }








        //Get answers of a question 

        [ResponseType(typeof(AnswerViewModel))]
        public IHttpActionResult GetAnswersOfAQuestion(int qid)
        {
            List<AnswerViewModel> lstQuestions = new List<AnswerViewModel>();
            var answers = from r in db.Answers where r.QuestionId == qid select r;
            //  Answer answer = db.Answers.Find(id);
            if (answers == null)
            {
                return NotFound();
            }

            var an = answers.ToList().Select(answer => new AnswerViewModel
            {
                AnswerDetailPlainText = answer.AnswerDetailPlainText,
                AnswerDetailRichText=answer.AnswerDetailRichText,
                CreatedDate = answer.CreatedDate,
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                Score = 10,
                UserId = answer.User.UserName,
                UserName = answer.User.FullName
            });

            return Ok(an);
        }

        //Get answers of a user 

        [ResponseType(typeof(AnswerViewModel))]
        public IHttpActionResult GetAnswersOfAUser(string userName)
        {
            string uid = (from r in db.Users where r.UserName == userName select r.Id).FirstOrDefault();
            List<AnswerViewModel> lstQuestions = new List<AnswerViewModel>();
            var answers = from r in db.Answers where r.UserId == uid select r;
            if (answers == null)
            {
                return NotFound();
            }

            var an = answers.ToList().Select(answer => new AnswerViewModel
            {
                AnswerDetailPlainText = answer.AnswerDetailPlainText,
                AnswerDetailRichText = answer.AnswerDetailRichText,
                CreatedDate = answer.CreatedDate,
                Id = answer.Id,
                QuestionId = answer.QuestionId,
                UserId = answer.UserId,
                UserName = answer.User.FullName
            });

            return Ok(an);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnswerExists(int id)
        {
            return db.Answers.Count(e => e.Id == id) > 0;
        }

        public XmlDocument ShortenURL(string urlToShorten)
        {
            using (WebClient wb = new WebClient())
            {
                string data = string.Format("http://api.bitly.com/v3/shorten/?login={0}&apiKey={1}&longUrl={2}&format={3}",
                "shubham0987",                             // Your username
                "R_7e1ce7adf4078e7ab734fc77bf1fb5a1",                              // Your API key
                HttpUtility.UrlEncode(urlToShorten),         // Encode the url we want to shorten
                "xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(wb.DownloadString(data));

                return xmlDoc;
            }
        }
    }
}