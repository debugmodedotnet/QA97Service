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

namespace QA97Service.Controllers
{
    public class UserImagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/UserImages
        public IQueryable<UserImage> GetUserImages()
        {
            return db.UserImages;
        }

        // GET: api/UserImages/5
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult GetUserImage(int id)
        {
            UserImage userImage = db.UserImages.Find(id);
            if (userImage == null)
            {
                return NotFound();
            }

            return Ok(userImage);
        }

        // PUT: api/UserImages/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUserImage(int id, UserImage userImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userImage.Id)
            {
                return BadRequest();
            }

            db.Entry(userImage).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserImageExists(id))
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

        // POST: api/UserImages
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult PostUserImage(UserImage userImage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.UserImages.Add(userImage);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = userImage.Id }, userImage);
        }

        // DELETE: api/UserImages/5
        [ResponseType(typeof(UserImage))]
        public IHttpActionResult DeleteUserImage(int id)
        {
            UserImage userImage = db.UserImages.Find(id);
            if (userImage == null)
            {
                return NotFound();
            }

            db.UserImages.Remove(userImage);
            db.SaveChanges();

            return Ok(userImage);
        }

        //Custom Call

        [HttpGet]
        public string GetParticularUserImage(string username)
        {
            string idOfUser = (from r in db.Users where r.UserName == username select r.Id).FirstOrDefault();
            return (from r in db.UserImages where r.UserId == idOfUser orderby r.ModifiedDate descending select r.ImageUrl).FirstOrDefault();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserImageExists(int id)
        {
            return db.UserImages.Count(e => e.Id == id) > 0;
        }
    }
}