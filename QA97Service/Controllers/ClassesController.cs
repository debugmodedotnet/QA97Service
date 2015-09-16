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
using System.Web.Http.Cors;

namespace QA97Service.Controllers
{
  
    public class ClassesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Classes
        //public IQueryable<Class> GetClasses()
        //{
        //    return db.Classes;
        //}
        public List<ClassViewModel> GetClasses()
        {
            var result = from r in db.Classes select new ClassViewModel { Name = r.Name, Id = r.Id };
            return result.ToList();
        }
        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult GetClass(int id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        // PUT: api/Classes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClass(int id, Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @class.Id)
            {
                return BadRequest();
            }

            db.Entry(@class).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
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

        // POST: api/Classes
        [ResponseType(typeof(Class))]
        public IHttpActionResult PostClass(Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Classes.Add(@class);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = @class.Id }, @class);
        }

        // DELETE: api/Classes/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(int id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            db.Classes.Remove(@class);
            db.SaveChanges();

            return Ok(@class);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(int id)
        {
            return db.Classes.Count(e => e.Id == id) > 0;
        }
    }
}