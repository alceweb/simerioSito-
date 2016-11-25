namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class documenti : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documentis",
                c => new
                    {
                        Documenti_Id = c.Int(nullable: false, identity: true),
                        Titolo = c.String(),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Documenti_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Documentis");
        }
    }
}
