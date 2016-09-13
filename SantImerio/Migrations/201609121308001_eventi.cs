namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class eventi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Eventis",
                c => new
                    {
                        Evento_Id = c.Int(nullable: false, identity: true),
                        Titolo = c.String(),
                        Descrizione = c.String(),
                        Data = c.DateTime(nullable: false),
                        DataI = c.DateTime(nullable: false),
                        DataF = c.DateTime(nullable: false),
                        Pubblica = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Evento_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Eventis");
        }
    }
}
