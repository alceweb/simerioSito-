namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class risposte : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComRisps",
                c => new
                    {
                        ComRisp_Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Commento_Id = c.Int(nullable: false),
                        Risposta = c.String(),
                        UId = c.String(),
                        Utente = c.String(),
                    })
                .PrimaryKey(t => t.ComRisp_Id)
                .ForeignKey("dbo.Commentis", t => t.Commento_Id, cascadeDelete: true)
                .Index(t => t.Commento_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComRisps", "Commento_Id", "dbo.Commentis");
            DropIndex("dbo.ComRisps", new[] { "Commento_Id" });
            DropTable("dbo.ComRisps");
        }
    }
}
