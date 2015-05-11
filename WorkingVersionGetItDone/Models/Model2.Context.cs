﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WorkingVersionGetItDone.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class nflff_dbEntities : DbContext
    {
        public nflff_dbEntities()
            : base("name=nflff_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<Bar> Bars { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<MemberGroup> MemberGroups { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Stadium> Stadiums { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamsTest> TeamsTests { get; set; }
        public virtual DbSet<WatchParty> WatchParties { get; set; }
    
        public virtual ObjectResult<ClickBarName_Result> ClickBarName(string bname)
        {
            var bnameParameter = bname != null ?
                new ObjectParameter("bname", bname) :
                new ObjectParameter("bname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClickBarName_Result>("ClickBarName", bnameParameter);
        }
    
        public virtual ObjectResult<string> ClickGroupName(string gname)
        {
            var gnameParameter = gname != null ?
                new ObjectParameter("gname", gname) :
                new ObjectParameter("gname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ClickGroupName", gnameParameter);
        }
    
        public virtual ObjectResult<ClickScheduledGame_Result> ClickScheduledGame(Nullable<int> gid)
        {
            var gidParameter = gid.HasValue ?
                new ObjectParameter("gid", gid) :
                new ObjectParameter("gid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ClickScheduledGame_Result>("ClickScheduledGame", gidParameter);
        }
    
        public virtual int deletemembergroup(Nullable<int> gid)
        {
            var gidParameter = gid.HasValue ?
                new ObjectParameter("gid", gid) :
                new ObjectParameter("gid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("deletemembergroup", gidParameter);
        }
    
        public virtual ObjectResult<getAllBars_Result> getAllBars()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getAllBars_Result>("getAllBars");
        }
    
        public virtual ObjectResult<string> getGroupName_for_unenrolledmember(string teamname, Nullable<int> memberid)
        {
            var teamnameParameter = teamname != null ?
                new ObjectParameter("teamname", teamname) :
                new ObjectParameter("teamname", typeof(string));
    
            var memberidParameter = memberid.HasValue ?
                new ObjectParameter("memberid", memberid) :
                new ObjectParameter("memberid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("getGroupName_for_unenrolledmember", teamnameParameter, memberidParameter);
        }
    
        public virtual ObjectResult<string> getgroups(Nullable<int> memberid)
        {
            var memberidParameter = memberid.HasValue ?
                new ObjectParameter("memberid", memberid) :
                new ObjectParameter("memberid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("getgroups", memberidParameter);
        }
    
        public virtual ObjectResult<getteams_Result> getteams(Nullable<int> memberid)
        {
            var memberidParameter = memberid.HasValue ?
                new ObjectParameter("memberid", memberid) :
                new ObjectParameter("memberid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getteams_Result>("getteams", memberidParameter);
        }
    }
}
