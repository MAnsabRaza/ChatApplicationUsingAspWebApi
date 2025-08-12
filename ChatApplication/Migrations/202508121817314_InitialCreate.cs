namespace ChatApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Modules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        current_date = c.DateTime(nullable: false),
                        module_icon = c.String(),
                        module_name = c.String(),
                        href = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        current_date = c.String(),
                        status = c.Boolean(nullable: false),
                        roleId = c.Int(nullable: false),
                        moduleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Modules", t => t.moduleId)
                .ForeignKey("dbo.Roles", t => t.roleId)
                .Index(t => t.roleId)
                .Index(t => t.moduleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        current_date = c.DateTime(nullable: false),
                        role_name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        current_date = c.DateTime(nullable: false),
                        name = c.String(),
                        email = c.String(),
                        password = c.String(),
                        phone_number = c.String(),
                        address = c.String(),
                        status = c.Boolean(nullable: false),
                        roleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.roleId)
                .Index(t => t.roleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "roleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "roleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "moduleId", "dbo.Modules");
            DropIndex("dbo.Users", new[] { "roleId" });
            DropIndex("dbo.Permissions", new[] { "moduleId" });
            DropIndex("dbo.Permissions", new[] { "roleId" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Permissions");
            DropTable("dbo.Modules");
        }
    }
}
