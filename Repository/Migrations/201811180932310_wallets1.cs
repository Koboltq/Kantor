namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wallets1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Wallet", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Wallet", "UserId");
        }
    }
}
