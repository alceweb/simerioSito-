namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class titoloimg1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImgTitolis",
                c => new
                    {
                        ImgTitolo_Id = c.Int(nullable: false, identity: true),
                        Img = c.String(),
                        ImgTitolo = c.String(),
                        Evento_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ImgTitolo_Id)
                .ForeignKey("dbo.Eventis", t => t.Evento_Id, cascadeDelete: true)
                .Index(t => t.Evento_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImgTitolis", "Evento_Id", "dbo.Eventis");
            DropIndex("dbo.ImgTitolis", new[] { "Evento_Id" });
            DropTable("dbo.ImgTitolis");
        }
    }
}
