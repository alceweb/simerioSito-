namespace SantImerio.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class orari : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrariMesseBars",
                c => new
                    {
                        Messe_Id = c.Int(nullable: false, identity: true),
                        Tipo = c.String(),
                        Descrizione = c.String(),
                    })
                .PrimaryKey(t => t.Messe_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.OrariMesseBars");
        }
    }
}
