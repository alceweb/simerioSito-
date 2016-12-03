namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pastorale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventis", "Pastorale", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventis", "Pastorale");
        }
    }
}
