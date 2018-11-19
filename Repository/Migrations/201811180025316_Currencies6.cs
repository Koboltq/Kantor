namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currencies6 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Currency", "PurchasePrice", c => c.Single(nullable: false));
            AlterColumn("dbo.Currency", "SalesPrice", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Currency", "SalesPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Currency", "PurchasePrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
