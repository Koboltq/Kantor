namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Wallet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wallet",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Currency = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Wallet");
        }
    }
}
