using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WorkingVersionGetItDone.SQLDataStuff;

namespace WorkingVersionGetItDone.Controllers
{
    public class MembersController : ApiController
    {
        private GatoradeShowerDB db = new GatoradeShowerDB();

        // GET: api/Members
        public IQueryable<Member> GetMembers()
        {
            return db.Members;
        }

        // GET: api/Members/5
        [ResponseType(typeof(Member))]
        [HttpGet] //adding http annotations
        public async Task<IHttpActionResult> GetMember(int id)
        {
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        //GET: api/Members?MemberStr={memberStr}
        //[ResponseType(typeof(Member))]
        //public async Task<T> GetMemberIDByName<T>(String MemberStr) where T : struct
        //{
        //    //var member = JsonConvert.DeserializeObject<Member>(MemberStr);
        //    //Member member = await db.Members.FindAsync(MemberStr);
        //    //var userId = ...;
        //    var member = await db.Members.Where(x => x.UserName == MemberStr).ToListAsync();
        //    if (member == null)
        //    {
        //        return (T)Convert.ChangeType(NotFound(), typeof(T));
        //    }
        //    //return member[0].MemberID.ToString();
        //    return (T)Convert.ChangeType(member[0].MemberID, typeof(T));
        //    //return member[0].MemberID; //hopefully gets id of first member in async list and returns it
        //    //return Ok(member);
        //}

        //GET: api/Members?MemberStr={memberStr}
        [ResponseType(typeof(Member))]
        [HttpGet]
        public async Task<IHttpActionResult> GetMemberIDByName(string MemberStr)
        {
            var member = await db.Members.Where(x => x.UserName == MemberStr).ToListAsync();
            if (member == null)
            {
                return NotFound();
            }
            return Ok(member[0].MemberID);
        }

        // PUT: api/Members/5
        [ResponseType(typeof(void))]
        [HttpPut]
        public async Task<IHttpActionResult> PutMember(int id, Member member)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != member.MemberID)
            {
                return BadRequest();
            }

            db.Entry(member).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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
        #region commentedOutApi
        // POST: api/Members
        //[ResponseType(typeof(Member))]
        //public async Task<IHttpActionResult> PostMember(Member member)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Members.Add(member);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = member.MemberID }, member);
        //}
        #endregion

        //modified post method to accept json string
        // POST: api/Members
        [ResponseType(typeof(Member))]
        [HttpPost]
        public async Task<IHttpActionResult> PostMember(String NewMember) //warning on this line
        {
            Member member = JsonConvert.DeserializeObject<Member>(NewMember);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.Members.Add(member);
                await db.SaveChangesAsync(); //changed from db.SaveChanges()
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

            return CreatedAtRoute("DefaultApi", new { id = member.MemberID }, member);
        }

        // DELETE: api/Members/5
        [ResponseType(typeof(Member))]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteMember(int id)
        {
            Member member = await db.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            db.Members.Remove(member);
            await db.SaveChangesAsync();

            return Ok(member);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MemberExists(int id)
        {
            return db.Members.Count(e => e.MemberID == id) > 0;
        }
    }
}