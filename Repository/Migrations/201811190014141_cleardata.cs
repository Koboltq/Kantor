namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cleardata : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Classifieds_Category", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Classifieds_Category", "ClassifiedId", "dbo.Classifieds");
            DropForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Classifieds_Category", new[] { "CategoryId" });
            DropIndex("dbo.Classifieds_Category", new[] { "ClassifiedId" });
            DropIndex("dbo.Classifieds", new[] { "UserId" });
            DropTable("dbo.Category");
            DropTable("dbo.Classifieds_Category");
            DropTable("dbo.Classifieds");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Classifieds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 500),
                        Title = c.String(maxLength: 72),
                        AddedDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Classifieds_Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        ClassifiedId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ParentId = c.Int(nullable: false),
                        MetaTitle = c.String(maxLength: 72),
                        MetaDescription = c.String(maxLength: 160),
                        MetaWords = c.String(maxLength: 160),
                        Content = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Classifieds", "UserId");
            CreateIndex("dbo.Classifieds_Category", "ClassifiedId");
            CreateIndex("dbo.Classifieds_Category", "CategoryId");
            AddForeignKey("dbo.Classifieds", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Classifieds_Category", "ClassifiedId", "dbo.Classifieds", "Id");
            AddForeignKey("dbo.Classifieds_Category", "CategoryId", "dbo.Category", "Id");
        }
    }
}
