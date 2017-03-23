namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rispemail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComRisps", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ComRisps", "Email");
        }
    }
}
