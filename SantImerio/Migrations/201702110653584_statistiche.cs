namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class statistiche : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Statistiches",
                c => new
                    {
                        Statistiche_Id = c.Int(nullable: false, identity: true),
                        Data = c.DateTime(nullable: false),
                        Ip = c.String(),
                        Pagina = c.String(),
                        UId = c.String(),
                        UName = c.String(),
                    })
                .PrimaryKey(t => t.Statistiche_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Statistiches");
        }
    }
}
