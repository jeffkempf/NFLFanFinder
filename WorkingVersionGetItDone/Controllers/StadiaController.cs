using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WorkingVersionGetItDone.SQLDataStuff;

namespace WorkingVersionGetItDone.Controllers
{
    public class StadiaController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/Stadia
        public IQueryable<Stadium> GetStadia()
        {
            return db.Stadia;
        }

        // GET: api/Stadia/5
        [ResponseType(typeof(Stadium))]
        public async Task<IHttpActionResult> GetStadium(int id)
        {
            Stadium stadium = await db.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }

            return Ok(stadium);
        }

        // PUT: api/Stadia/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStadium(int id, Stadium stadium)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != stadium.StadiumID)
            {
                return BadRequest();
            }

            db.Entry(stadium).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StadiumExists(id))
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

        // POST: api/Stadia
        [ResponseType(typeof(Stadium))]
        public async Task<IHttpActionResult> PostStadium(Stadium stadium)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stadia.Add(stadium);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = stadium.StadiumID }, stadium);
        }

        //modified post method to accept json string
        // POST: api/Bars
        [ResponseType(typeof(Stadium))]
        public async Task<IHttpActionResult> PostStadium(String stadiumStr)
        {
            var stadium = JsonConvert.DeserializeObject<Stadium>(stadiumStr);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stadia.Add(stadium);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = stadium.StadiumID }, stadium);
        }

        // DELETE: api/Stadia/5
        [ResponseType(typeof(Stadium))]
        public async Task<IHttpActionResult> DeleteStadium(int id)
        {
            Stadium stadium = await db.Stadia.FindAsync(id);
            if (stadium == null)
            {
                return NotFound();
            }

            db.Stadia.Remove(stadium);
            await db.SaveChangesAsync();

            return Ok(stadium);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StadiumExists(int id)
        {
            return db.Stadia.Count(e => e.StadiumID == id) > 0;
        }
    }
}