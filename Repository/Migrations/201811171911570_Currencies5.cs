namespace Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Currencies5 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Currency", "UpdatedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Currency", "UpdatedDate", c => c.DateTime(nullable: false));
        }
    }
}
