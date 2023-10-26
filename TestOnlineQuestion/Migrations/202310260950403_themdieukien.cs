namespace TestOnlineQuestion.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class themdieukien : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "HoDem", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Ten", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Password", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.User", "Email", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "Email", c => c.String(maxLength: 50));
            AlterColumn("dbo.User", "Password", c => c.String(maxLength: 50));
            AlterColumn("dbo.User", "Ten", c => c.String(maxLength: 50));
            AlterColumn("dbo.User", "HoDem", c => c.String(maxLength: 50));
        }
    }
}
