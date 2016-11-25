namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sottotitolo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Eventis", "SottoTitolo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Eventis", "SottoTitolo");
        }
    }
}
