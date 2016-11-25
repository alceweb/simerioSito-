namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pubblicaHome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventis", "Home", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventis", "Home");
        }
    }
}
