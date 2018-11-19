namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wallets3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Wallet", "BuyAndSell_BuyQuantity");
            DropColumn("dbo.Wallet", "BuyAndSell_SellQuantity");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Wallet", "BuyAndSell_SellQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Wallet", "BuyAndSell_BuyQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
