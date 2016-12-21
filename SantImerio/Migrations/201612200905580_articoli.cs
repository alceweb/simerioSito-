namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class articoli : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articolis",
                c => new
                    {
                        Articolo_Id = c.Int(nullable: false, identity: true),
                        Titolo = c.String(),
                        SottoTitolo = c.String(),
                        Testo = c.String(),
                        Data = c.DateTime(nullable: false),
                        Pubblica = c.Boolean(nullable: false),
                        Autore = c.String(),
                    })
                .PrimaryKey(t => t.Articolo_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Articolis");
        }
    }
}
