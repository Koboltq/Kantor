namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currencies2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Currency", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Currency", "UserId");
            AddForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Currency", new[] { "UserId" });
            DropColumn("dbo.Currency", "UserId");
        }
    }
}
