namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class volantino : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventis", "Volantino", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventis", "Volantino");
        }
    }
}
