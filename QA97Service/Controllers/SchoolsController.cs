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
    public class SchoolsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Schools
        public List<SchoolViewModel> GetSchools()
        {
            List<SchoolViewModel> schools = new List<SchoolViewModel>();
            SchoolViewModel s;
            var result = from r in db.Schools select r;
            foreach (var r in result)
            {
                s = new SchoolViewModel
                {
                    Address = r.Address,
                    Id = r.Id,
                    LogoUrl = r.LogoUrl,
                    Name = r.Name
                };

                schools.Add(s);
            }
            return schools;
        }

        // GET: api/Schools/5
        [ResponseType(typeof(School))]
        public IHttpActionResult GetSchool(int id)
        {
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return NotFound();
            }

            return Ok(school);
        }

        // PUT: api/Schools/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSchool(int id, School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != school.Id)
            {
                return BadRequest();
            }

            db.Entry(school).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolExists(id))
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

        // POST: api/Schools
        [ResponseType(typeof(School))]
        public IHttpActionResult PostSchool(School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Schools.Add(school);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = school.Id }, school);
        }

        // DELETE: api/Schools/5
        [ResponseType(typeof(School))]
        public IHttpActionResult DeleteSchool(int id)
        {
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return NotFound();
            }

            db.Schools.Remove(school);
            db.SaveChanges();

            return Ok(school);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SchoolExists(int id)
        {
            return db.Schools.Count(e => e.Id == id) > 0;
        }
    }
}