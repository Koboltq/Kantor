namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currencies3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currency", "SalesPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Currency", "SellPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Currency", "SellPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Currency", "SalesPrice");
        }
    }
}
