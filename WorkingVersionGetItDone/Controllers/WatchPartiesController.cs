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
    public class WatchPartiesController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/WatchParties
        public IQueryable<WatchParty> GetWatchParties()
        {
            return db.WatchParties;
        }

        // GET: api/WatchParties/5
        [ResponseType(typeof(WatchParty))]
        public async Task<IHttpActionResult> GetWatchParty(int id)
        {
            WatchParty watchParty = await db.WatchParties.FindAsync(id);
            if (watchParty == null)
            {
                return NotFound();
            }

            return Ok(watchParty);
        }

        // PUT: api/WatchParties/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWatchParty(int id, WatchParty watchParty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != watchParty.PartyID)
            {
                return BadRequest();
            }

            db.Entry(watchParty).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WatchPartyExists(id))
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

        // POST: api/WatchParties
        [ResponseType(typeof(WatchParty))]
        public async Task<IHttpActionResult> PostWatchParty(WatchParty watchParty)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WatchParties.Add(watchParty);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = watchParty.PartyID }, watchParty);
        }

        //modified post method to accept json string
        // POST: api/Bars
        [ResponseType(typeof(WatchParty))]
        public async Task<IHttpActionResult> PostWatchParty(String partyStr)
        {
            var party = JsonConvert.DeserializeObject<WatchParty>(partyStr); //wtf? why doesn't this work?
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.WatchParties.Add(party);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = party.PartyID }, party);
        }

        // DELETE: api/WatchParties/5
        [ResponseType(typeof(WatchParty))]
        public async Task<IHttpActionResult> DeleteWatchParty(int id)
        {
            WatchParty watchParty = await db.WatchParties.FindAsync(id);
            if (watchParty == null)
            {
                return NotFound();
            }

            db.WatchParties.Remove(watchParty);
            await db.SaveChangesAsync();

            return Ok(watchParty);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WatchPartyExists(int id)
        {
            return db.WatchParties.Count(e => e.PartyID == id) > 0;
        }
    }
}