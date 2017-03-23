namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class commemail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commentis", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commentis", "Email");
        }
    }
}
