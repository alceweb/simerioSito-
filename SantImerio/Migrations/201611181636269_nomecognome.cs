namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nomecognome : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Nome", c => c.String());
            AddColumn("dbo.AspNetUsers", "Cognome", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Cognome");
            DropColumn("dbo.AspNetUsers", "Nome");
        }
    }
}
