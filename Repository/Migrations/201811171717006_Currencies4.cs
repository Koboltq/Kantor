namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currencies4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Classifieds", new[] { "UserId" });
            DropIndex("dbo.Currency", new[] { "UserId" });
            AddColumn("dbo.Currency", "UpdatedDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Classifieds", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Currency", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Classifieds", "UserId");
            CreateIndex("dbo.Currency", "UserId");
            AddForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Currency", new[] { "UserId" });
            DropIndex("dbo.Classifieds", new[] { "UserId" });
            AlterColumn("dbo.Currency", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Classifieds", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Currency", "UpdatedDate");
            CreateIndex("dbo.Currency", "UserId");
            CreateIndex("dbo.Classifieds", "UserId");
            AddForeignKey("dbo.Currency", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
