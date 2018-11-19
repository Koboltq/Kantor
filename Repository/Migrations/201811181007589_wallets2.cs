namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wallets2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wallet", "BuyAndSell_BuyQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Wallet", "BuyAndSell_SellQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wallet", "BuyAndSell_SellQuantity");
            DropColumn("dbo.Wallet", "BuyAndSell_BuyQuantity");
        }
    }
}
