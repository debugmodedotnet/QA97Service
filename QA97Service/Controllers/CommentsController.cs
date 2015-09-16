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
using Microsoft.AspNet.Identity;
using QA97Service.ViewModel;

namespace QA97Service.Controllers
{
    public class CommentsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Comments
        public IQueryable<Comment> GetComments()
        {
            return db.Comments;
        }

        // GET: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult GetComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/Comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutComment(int id, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // POST: api/Comments
        [ResponseType(typeof(Comment))]
        public IHttpActionResult PostComment(Comment comment)
        {
            string UserId = (from r in db.Users where r.UserName == comment.CreatedBy select r.Id).FirstOrDefault();
            comment.CreatedDate = DateTime.Now;
            comment.ModifiedDate = DateTime.Now;
            comment.ModifiedBy = comment.CreatedBy;
            comment.UserId = UserId;

            db.Comments.Add(comment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [ResponseType(typeof(Comment))]
        public IHttpActionResult DeleteComment(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentExists(int id)
        {
            return db.Comments.Count(e => e.Id == id) > 0;
        }

        //Custom Call

        [HttpGet]
        [ResponseType(typeof(CommentsViewModel))]
        public IHttpActionResult GetCommentsbyQuestion(int id)
        {

            List<CommentsViewModel> lstQuestions = new List<CommentsViewModel>();
            var comments = from r in db.Comments where r.QuestionId == id select r;

            if (comments == null)
            {
                return NotFound();
            }

            var cm = comments.ToList().Select(comment => new CommentsViewModel
            {
                CommentText = comment.CommentText,
                CreatedDate = comment.CreatedDate,
                CreatedBy = comment.CreatedBy,
                QuestionId = comment.QuestionId,
                Id = comment.Id
            });

            return Ok(cm);
        }

    }
}