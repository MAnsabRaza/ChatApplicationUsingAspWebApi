namespace ChatApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addTableChat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chats",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        current_date = c.DateTime(nullable: false),
                        userId = c.Int(nullable: false),
                        sessionId = c.Guid(nullable: false),
                        message = c.String(),
                        response = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.userId)
                .Index(t => t.userId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chats", "userId", "dbo.Users");
            DropIndex("dbo.Chats", new[] { "userId" });
            DropTable("dbo.Chats");
        }
    }
}
