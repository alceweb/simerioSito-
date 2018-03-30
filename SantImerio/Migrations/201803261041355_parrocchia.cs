namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class parrocchia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incarichis", "Parrocchia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Incarichis", "Parrocchia");
        }
    }
}
