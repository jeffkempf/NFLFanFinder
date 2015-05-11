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
using System.Data.Linq.Mapping;
using Newtonsoft.Json;

namespace WorkingVersionGetItDone.Controllers
{
    [Table(Name = "MemberGroups")]
    public class MemberGroupsController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/MemberGroups
        public IQueryable<MemberGroup> GetMemberGroups()
        {
            return db.MemberGroups;
        }

        // GET: api/MemberGroups/5
        [ResponseType(typeof(MemberGroup))]
        public async Task<IHttpActionResult> GetMemberGroup(int id)
        {
            MemberGroup memberGroup = await db.MemberGroups.FindAsync(id);
            if (memberGroup == null)
            {
                return NotFound();
            }

            return Ok(memberGroup);
        }

        //GET: api/MemberGroups?mg=5
        /* gets all teams member interested in by passing memberID into MemberGroups, gets GameID of each group member
         part of, then creates a list of all home and away teams for those games and returns the list of teams*/
        public List<string> GetTeamsByMemberGroup(int mg)
        {
            var teams = db.getteams(mg);
            List<string> results = new List<string>();
            foreach (var homeAway in teams)
            {
                results.Add(homeAway.HomeTeam);
                results.Add(homeAway.AwayTeam);
            }
            return results;
        }

        // PUT: api/MemberGroups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMemberGroup(int id, MemberGroup memberGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberGroup.MemGrpID)
            {
                return BadRequest();
            }

            db.Entry(memberGroup).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberGroupExists(id))
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

        // POST: api/MemberGroups
        [ResponseType(typeof(MemberGroup))]
        public async Task<IHttpActionResult> PostMemberGroup(MemberGroup memberGroup)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MemberGroups.Add(memberGroup);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = memberGroup.MemGrpID }, memberGroup);
        }

        // POST: api/MemberGroups?newMG
        [ResponseType(typeof(MemberGroup))]
        public async Task<IHttpActionResult> PostMemberGroup(string newMG)
        {
            var memberGroup = JsonConvert.DeserializeObject<MemberGroup>(newMG);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MemberGroups.Add(memberGroup);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = memberGroup.MemGrpID }, memberGroup);
        }

        // DELETE: api/MemberGroups/5
        [ResponseType(typeof(MemberGroup))]
        public async Task<IHttpActionResult> DeleteMemberGroup(int id)
        {
            MemberGroup memberGroup = await db.MemberGroups.FindAsync(id);
            if (memberGroup == null)
            {
                return NotFound();
            }

            db.MemberGroups.Remove(memberGroup);
            await db.SaveChangesAsync();

            return Ok(memberGroup);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberGroupExists(int id)
        {
            return db.MemberGroups.Count(e => e.MemGrpID == id) > 0;
        }
    }
}