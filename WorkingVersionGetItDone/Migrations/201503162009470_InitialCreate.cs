namespace WorkingVersionGetItDone.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bars",
                c => new
                    {
                        BarID = c.Int(nullable: false, identity: true),
                        BarName = c.String(nullable: false, maxLength: 255),
                        BarStreet = c.String(nullable: false, maxLength: 255),
                        BarCity = c.String(nullable: false, maxLength: 50),
                        BarState = c.String(nullable: false, maxLength: 50),
                        BarPhone = c.String(nullable: false, maxLength: 50),
                        BarZip = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BarID);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventID = c.Int(nullable: false, identity: true),
                        EventType = c.String(nullable: false, maxLength: 100),
                        EventLocationID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventID);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        GameID = c.Int(nullable: false, identity: true),
                        HomeTeam = c.String(nullable: false, maxLength: 100),
                        AwayTeam = c.String(nullable: false, maxLength: 100),
                        StadiumID = c.Int(nullable: false),
                        GameTime = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "timestamp"),
                        GameDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GameID);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        GroupID = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 50),
                        GroupOwnerID = c.Int(nullable: false),
                        CreationDate = c.DateTime(),
                        GameID = c.Int(nullable: false),
                        EventType = c.String(nullable: false, maxLength: 50),
                        EventID = c.Int(nullable: false),
                        Status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.GroupID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 32),
                        Email = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        State = c.String(nullable: false, maxLength: 30),
                        Zip = c.String(nullable: false, maxLength: 255),
                        Phone = c.String(nullable: false, maxLength: 255),
                        FavTeamID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberID);
            
            CreateTable(
                "dbo.Stadium",
                c => new
                    {
                        StadiumID = c.Int(nullable: false, identity: true),
                        StadiumName = c.String(nullable: false, maxLength: 50),
                        StadiumAddress = c.String(nullable: false, maxLength: 50),
                        StadiumCity = c.String(nullable: false, maxLength: 50),
                        StadiumState = c.String(nullable: false, maxLength: 50),
                        StadiumZip = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.StadiumID);
            
            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        TeamID = c.Int(nullable: false, identity: true),
                        TeamName = c.String(nullable: false, maxLength: 50),
                        TeamCity = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.TeamID);
            
            CreateTable(
                "dbo.TeamsTest",
                c => new
                    {
                        teamID = c.Int(nullable: false),
                        teamName = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.teamID);
            
            CreateTable(
                "dbo.WatchParties",
                c => new
                    {
                        PartyID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        PartyStreet = c.String(nullable: false, maxLength: 50),
                        PartyCity = c.String(nullable: false, maxLength: 50),
                        PartyState = c.String(nullable: false, maxLength: 50),
                        Private = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartyID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WatchParties");
            DropTable("dbo.TeamsTest");
            DropTable("dbo.Teams");
            DropTable("dbo.Stadium");
            DropTable("dbo.Members");
            DropTable("dbo.Groups");
            DropTable("dbo.Games");
            DropTable("dbo.Events");
            DropTable("dbo.Bars");
        }
    }
}
