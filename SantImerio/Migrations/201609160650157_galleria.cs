namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class galleria : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventis", "Galleria", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventis", "Galleria");
        }
    }
}
