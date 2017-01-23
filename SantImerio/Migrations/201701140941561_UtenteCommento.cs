namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UtenteCommento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commentis", "Utente", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commentis", "Utente");
        }
    }
}
