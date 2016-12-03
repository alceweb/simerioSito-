namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class incarichi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incarichis",
                c => new
                    {
                        Incarichi_Id = c.Int(nullable: false, identity: true),
                        Incarico = c.String(),
                        Nome = c.String(),
                        Cognome = c.String(),
                        Telefono = c.String(),
                        Email = c.String(),
                        Indirizzo = c.String(),
                        Città = c.String(),
                        Cap = c.String(),
                    })
                .PrimaryKey(t => t.Incarichi_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Incarichis");
        }
    }
}
