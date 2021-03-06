namespace WorkingVersionGetItDone.SQLDataStuff
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Validation;
    using System.Text;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using CodeFirstStoreFunctions;
    using System.Collections.Generic;

    public partial class GatoradeShowerDB : DbContext
    {
        public GatoradeShowerDB()
            : base("name=GatoradeShowerDB")
        {
        }

        public virtual DbSet<Bar> Bars { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Stadium> Stadia { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<TeamsTest> TeamsTests { get; set; }
        public virtual DbSet<WatchParty> WatchParties { get; set; }
        public virtual DbSet<MemberGroup> MemberGroups { get; set; }
        //public virtual DbSet<getteams_Results> TeamResults { get; set; } //adding to fix edmType not found error for getteams_Results

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Game>()
                .Property(e => e.GameTime)
                .IsFixedLength();
            */
            modelBuilder.Entity<TeamsTest>()
                .Property(e => e.teamName)
                .IsUnicode(false);

            //trying to fix edmType not found error for getteams_Results
            modelBuilder.ComplexType<getteams_Results>();

            //added the following code to support stored procedures
            modelBuilder.Conventions.Add(new FunctionsConvention<GatoradeShowerDB>("dbo"));
        }

        //hopefull this will finally allow me to see the error
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        //generated by EF when created separate data context so shouldn't need to modify this
        public virtual ObjectResult<string> ClickGroupName(string gname)
        {
            var gnameParameter = gname != null ?
                new ObjectParameter("gname", gname) :
                new ObjectParameter("gname", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ClickGroupName", gnameParameter);
        }

        public virtual ObjectResult<getteams_Results> getteams(Nullable<int> memberid)
        {
            var memberidParameter = memberid.HasValue ?
                new ObjectParameter("memberid", memberid) :
                new ObjectParameter("memberid", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<getteams_Results>("getteams", memberidParameter);
        }

        public virtual ObjectResult<string> getGroupName_for_unenrolledmember(string teamname, Nullable<int> memberid)
        {
            var teamnameParameter = teamname != null ?
                new ObjectParameter("teamname", teamname) :
                new ObjectParameter("teamname", typeof(string));

            var memberidParameter = memberid.HasValue ?
                new ObjectParameter("memberid", memberid) :
                new ObjectParameter("memberid", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("getGroupName_for_unenrolledmember", 
                teamnameParameter, memberidParameter);
        }

    }
}
