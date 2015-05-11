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
    public class BarsController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/Bars
       // [Route("http://gatoradeshower.azurewebsites.net/api/bars/All")]
        public IQueryable<Bar> GetBars()
        {
            return db.Bars;
        }

        // GET: api/Bars/5
        [ResponseType(typeof(Bar))]
        public async Task<IHttpActionResult> GetBar(int id)
        {
            Bar bar = await db.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            // return Ok(bar);
            return Ok(bar);
        }

        // PUT: api/Bars/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBar(int id, Bar bar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bar.BarID)
            {
                return BadRequest();
            }

            db.Entry(bar).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BarExists(id))
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

        //// POST: api/Bars
        //[ResponseType(typeof(Bar))]
        //public async Task<IHttpActionResult> PostBar(Bar bar)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Bars.Add(bar);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = bar.BarID }, bar);
        //}

        //modified post method to accept json string
        // POST: api/Bars
        [ResponseType(typeof(Bar))]
        public async Task<IHttpActionResult> PostBar(String barStr)
        {
            var bar = JsonConvert.DeserializeObject<Bar>(barStr);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bars.Add(bar);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bar.BarID }, bar);
        }

        // POST: api/Bars
        //[ResponseType(typeof(Bar))]
        //public async Task<IHttpActionResult> PostBars(List<Bar> bars)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Bars.AddRange(bars);
        //    await db.SaveChangesAsync();

        //    //return CreatedAtRoute("DefaultApi", new { id = bar.BarID }, bar);
        //}

        // DELETE: api/Bars/5
        [ResponseType(typeof(Bar))]
        public async Task<IHttpActionResult> DeleteBar(int id)
        {
            Bar bar = await db.Bars.FindAsync(id);
            if (bar == null)
            {
                return NotFound();
            }

            db.Bars.Remove(bar);
            await db.SaveChangesAsync();

            return Ok(bar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BarExists(int id)
        {
            return db.Bars.Count(e => e.BarID == id) > 0;
        }
    }
}