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

namespace QA97Service.Controllers
{
    public class QuestionsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Questions
        //public IQueryable<Question> GetQuestions()
        //{
        //    return db.Questions;
        //}

        // GET: api/Questions/5
        [ResponseType(typeof(QuestionViewModel))]
        public IHttpActionResult GetQuestion(int id)
        {
            Question r = db.Questions.Find(id);
            if (r == null)
            {
                return NotFound();
            }

            QuestionViewModel question = new QuestionViewModel
            {

                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,
                UserId = r.User.UserName,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText=r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count
            };

            return Ok(question);
        }

        // PUT: api/Questions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutQuestion(int id, Question question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != question.Id)
            {
                return BadRequest();
            }

            db.Entry(question).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        // POST: api/Questions
        [ResponseType(typeof(Question))]
        public IHttpActionResult PostQuestion(Question question)
        {
            
            string idOfUserAskingQuestion = (from r in db.Users where r.UserName==question.UserId select r.Id).FirstOrDefault();
            //ApplicationUser u = db.Users.Find(question.)
            question.CreatedBy = "Foo";
            question.ModifiedBy = "Foo";
            question.CreatedDate = DateTime.Now;
            question.ModifiedDate = DateTime.Now;
            question.UserId = idOfUserAskingQuestion;
            db.Questions.Add(question);

            //Adding Points to User
            //UserPoint pointToUser = new UserPoint();
            //pointToUser.CreatedBy = "Foo";
            //pointToUser.ModifiedBy = "Foo";
            //pointToUser.CreatedDate = DateTime.Now;
            //pointToUser.ModifiedDate = DateTime.Now;

            //var userInPointTable = (from r in db.UserPoints where r.UserId == idOfUserAskingQuestion select r).FirstOrDefault();
            //if (userInPointTable == null)
            //{
            //    pointToUser.UserId = idOfUserAskingQuestion;
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
           

            return CreatedAtRoute("DefaultApi", new { id = question.Id }, question);
        }

        // DELETE: api/Questions/5
        [ResponseType(typeof(Question))]
        public IHttpActionResult DeleteQuestion(int id)
        {
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return NotFound();
            }

            db.Questions.Remove(question);
            db.SaveChanges();

            return Ok(question);
        }



        // Custom API methods 

       
        // API to return List of Questions for given grade and subject. You can also pass the page number and number of records to fetch
        [HttpGet]
        public List<QuestionViewModel> ListQuestions(int pageno, int pagesize, int? gid, int? sid)
        {
            List<QuestionViewModel> lstQuestions = new List<QuestionViewModel>();

            if (gid == -1)
            {
                gid = null;

            }
            if (sid == -1)
            {
                sid = null;
            }

            //Fetching the results from the DB 
          
            var result = ((from o in db.Questions
                           where (gid == null || o.ClassId == gid)
                           && (sid == null || o.SubjectId == sid)
                           orderby o.Id
                           select o).Skip((pageno - 1) * pagesize).Take(pagesize)).ToList();
            
            //Converting Questions to QuestionsViewModel 
            //Modify the query . It has the performance issues. 

            var questions = result.Select(r => new QuestionViewModel
            {

                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,               
                UserId = r.UserId,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText = r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count

            }).ToList();
            return questions;
        }


        // API to return List of Questions for given subject. 

        [HttpGet]
        public List<QuestionViewModel> QuestionsinSubject(int sid)
        {
            List<QuestionViewModel> lstQuestions = new List<QuestionViewModel>();
            //fetching questions from Database            

            var QuestionsOfASubject = (from r in db.Questions where r.SubjectId == sid select r).ToList();


            //Converting Questions to QuestionsViewModel 
            //Modify the query . It has the performance issues. 
            var questions = QuestionsOfASubject.Select(r => new QuestionViewModel
            {

                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,
                UserId = r.UserId,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText = r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count

            }).ToList();


            return questions;
        }


        // API to return List of Questions for given grades. 

        [HttpGet]
        public List<QuestionViewModel> QuestionsinGrades(int gid)
        {
            List<QuestionViewModel> lstQuestions = new List<QuestionViewModel>();

            var QuestionsOfASubject = (from r in db.Questions where r.ClassId == gid select r).ToList();
            //Converting Questions to QuestionsViewModel 
            //Modify the query . It has the performance issues. 
            var questions = QuestionsOfASubject.Select(r => new QuestionViewModel
            {

                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,
                UserId = r.UserId,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText = r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count

            }).ToList();


            return questions;
        }

        // API to return List of all Questions.

        [HttpGet]
        public List<QuestionViewModel> GetQuestions()
        {



            List<QuestionViewModel> lstQuestions = new List<QuestionViewModel>();

            //Converting Questions to QuestionsViewModel 
            //Modify the query . It has the performance issues. 
            var questions = db.Questions.Select(r => new QuestionViewModel
            {
                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,
                UserId = r.UserId,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText = r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count

            }).ToList();


            return questions;
        }


        [HttpGet]
        public int CastVote(int score, string userid, int questionid)
        {
          
            
            string idOfUserVoting = (from r in db.Users where r.UserName == userid select r.Id).FirstOrDefault();
            if (idOfUserVoting != null)
            {
               
                    QuestionVote voteExist = (from r in db.QuestionVotes where r.UserId == idOfUserVoting && r.QuestionId == questionid select r).FirstOrDefault();
                
               
                   
                    if (voteExist == null)
                {
                    QuestionVote votetoinsert = new QuestionVote();
                    votetoinsert.CreatedDate = DateTime.Now;
                    votetoinsert.ModifiedBy = "Foo";
                    votetoinsert.ModifiedDate = DateTime.Now;
                    votetoinsert.CreatedBy = "Foo";
                    votetoinsert.QuestionId = questionid;
                    votetoinsert.Score = score;
                    votetoinsert.UserId = idOfUserVoting;
                    db.QuestionVotes.Add(votetoinsert);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch(Exception ex)
                    {
                        var r = ex.Message;
                    }
                }
                else
                {
                    if (voteExist.Score != score)
                    {
                        voteExist.Score = score;
                        db.SaveChanges();
                    }
                    else
                    {
                        return 0;
                    }
                }
                return score;
            }
            else
            {
                return -99; 
            }
        }




        //Questions asked by a user
        [HttpGet]
        public List<QuestionViewModel> QuestionsbyUser(string userName)
        {
            List<QuestionViewModel> lstQuestions = new List<QuestionViewModel>();
            //fetching questions from Database            
            string uid = (from r in db.Users where r.UserName == userName select r.Id).FirstOrDefault();

            var QuestionsOfAUser = (from r in db.Questions where r.UserId == uid select r).ToList();
            //Converting Questions to QuestionsViewModel 
            //Modify the query . It has the performance issues. 
            var questions = QuestionsOfAUser.Select(r => new QuestionViewModel
            {

                Id = r.Id,
                ClassId = r.ClassId,
                ClassName = r.Class.Name,
                UserId = r.UserId,
                UserName = r.User.FullName,
                CreatedDate = r.CreatedDate,
                DetailPlainText = r.QuestionDetailPlainText,
                DetailRichText = r.QuestionDetailRichText,
                Score = r.QuestionVotes.Sum(x => x.Score),
                SubjectId = r.SubjectId,
                SubjectName = r.Subject.Name,
                Title = r.QuestionTitle,
                NumberOfAnswers = r.Answers.Count

            }).ToList();


            return questions;
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionExists(int id)
        {
            return db.Questions.Count(e => e.Id == id) > 0;
        }
    }
}