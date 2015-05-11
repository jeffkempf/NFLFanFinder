using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WorkingVersionGetItDone.SQLDataStuff;

namespace WorkingVersionGetItDone.Controllers
{
    public class GroupsController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/Groups
        public IQueryable<Group> GetGroups()
        {
            return db.Groups;
        }

        // GET: api/Groups/5
        [ResponseType(typeof(Group))]
        public async Task<IHttpActionResult> GetMemberGroup(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // GET: api/Groups?teamName={teamName}&memID={memID}
        [ResponseType(typeof(Group))]
        [HttpGet]
        public List<string> GetGroupsForFollowedTeams(string teamName, int memID) //was Task<IHttpActionResult> for return value
        {
            var groups = db.getGroupName_for_unenrolledmember(teamName, memID);
            List<string> results = new List<string>();
            foreach (var group in groups)
            {
                results.Add(group);

            }
            return results;
        }

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

        // PUT: api/Groups/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.GroupID)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
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

        // POST: api/Groups
        [ResponseType(typeof(Group))]
        [HttpPost]
        public async Task<IHttpActionResult> PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = group.GroupID }, group);
        }

        //modified post method to accept json string
        // POST: api/Groups
        [ResponseType(typeof(Group))]
        [HttpPost]
        public async Task<IHttpActionResult> PostGroup(String groupStr)
        {
            Group group = JsonConvert.DeserializeObject<Group>(groupStr);//changed from var to Group
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //added from equiv memberController method
            try
            {
                db.Groups.Add(group);
                await db.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Console.WriteLine("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            //db.Groups.Add(group);
            //await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = group.GroupID }, group);
        }

        // DELETE: api/Groups/5
        [ResponseType(typeof(Group))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteGroup(int id)
        {
            Group group = await db.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
            await db.SaveChangesAsync();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.GroupID == id) > 0;
        }

        //hopefully will call stored procedure to get members for a group. Changing from IQueryable<string> to List<string>
        public List<string> GetMembersByGroup(String groupName)
        {

            //currently works correctly
            var members = db.ClickGroupName(groupName);
            List<string> results = new List<string>();
            foreach (var member in members)
            {
                results.Add(member);

            }
            return results;
        }

        //GET: api/Groups?byName={byName}
        [ResponseType(typeof(Group))]
        [HttpGet]
        public async Task<IHttpActionResult> GetGroupIDByName(string byName)
        {
            var group = await db.Groups.Where(x => x.GroupName == byName).ToListAsync();
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group[0].GroupID);
        }

    }
}