namespace CC.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrganizationAdditions : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetRoles", newName: "Role");
            RenameTable(name: "dbo.AspNetUserRoles", newName: "UserRole");
            RenameTable(name: "dbo.AspNetUsers", newName: "User");
            RenameTable(name: "dbo.AspNetUserClaims", newName: "UserClaim");
            RenameTable(name: "dbo.AspNetUserLogins", newName: "UserLogin");
            RenameColumn(table: "dbo.User", name: "Id", newName: "UserId");
            CreateTable(
                "dbo.Organization",
                c => new
                    {
                        OrganizationId = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 4000, unicode: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedByUserId = c.Guid(),
                        UpdatedByUserId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        DatabaseName = c.String(maxLength: 50, unicode: false),
                        Website = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.OrganizationId)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "dbo.UserOrganizationRole",
                c => new
                    {
                        UserOrganizationRoleId = c.Guid(nullable: false),
                        OrganizationRoleId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserOrganizationRoleId)
                .ForeignKey("dbo.OrganizationRole", t => t.OrganizationRoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.OrganizationRoleId }, unique: true, name: "IX_OrganizationRoleAndUser");
            
            CreateTable(
                "dbo.OrganizationRole",
                c => new
                    {
                        OrganizationRoleId = c.Guid(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedByUserId = c.Guid(),
                        UpdatedByUserId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrganizationRoleId)
                .ForeignKey("dbo.Organization", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.CreatedByUserId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UpdatedByUserId)
                .Index(t => t.OrganizationId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            AddColumn("dbo.Role", "Description", c => c.String());
            AddColumn("dbo.User", "ActiveOrganizationId", c => c.Guid());
            AddColumn("dbo.User", "FirstName", c => c.String(maxLength: 50));
            AddColumn("dbo.User", "LastName", c => c.String(maxLength: 50));
            AddColumn("dbo.User", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "CreatedByUserId", c => c.Guid());
            AddColumn("dbo.User", "UpdatedByUserId", c => c.Guid());
            AddColumn("dbo.User", "UpdatedDate", c => c.DateTime());
            AddColumn("dbo.User", "CreatedDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.User", "ActiveOrganizationId");
            CreateIndex("dbo.User", "CreatedByUserId");
            CreateIndex("dbo.User", "UpdatedByUserId");
            AddForeignKey("dbo.User", "ActiveOrganizationId", "dbo.Organization", "OrganizationId");
            AddForeignKey("dbo.User", "CreatedByUserId", "dbo.User", "UserId");
            AddForeignKey("dbo.User", "UpdatedByUserId", "dbo.User", "UserId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organization", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.Organization", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.UserOrganizationRole", "UserId", "dbo.User");
            DropForeignKey("dbo.UserOrganizationRole", "OrganizationRoleId", "dbo.OrganizationRole");
            DropForeignKey("dbo.OrganizationRole", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.OrganizationRole", "RoleId", "dbo.Role");
            DropForeignKey("dbo.OrganizationRole", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.OrganizationRole", "OrganizationId", "dbo.Organization");
            DropForeignKey("dbo.User", "UpdatedByUserId", "dbo.User");
            DropForeignKey("dbo.User", "CreatedByUserId", "dbo.User");
            DropForeignKey("dbo.User", "ActiveOrganizationId", "dbo.Organization");
            DropIndex("dbo.OrganizationRole", new[] { "UpdatedByUserId" });
            DropIndex("dbo.OrganizationRole", new[] { "CreatedByUserId" });
            DropIndex("dbo.OrganizationRole", new[] { "RoleId" });
            DropIndex("dbo.OrganizationRole", new[] { "OrganizationId" });
            DropIndex("dbo.UserOrganizationRole", "IX_OrganizationRoleAndUser");
            DropIndex("dbo.User", new[] { "UpdatedByUserId" });
            DropIndex("dbo.User", new[] { "CreatedByUserId" });
            DropIndex("dbo.User", new[] { "ActiveOrganizationId" });
            DropIndex("dbo.Organization", new[] { "UpdatedByUserId" });
            DropIndex("dbo.Organization", new[] { "CreatedByUserId" });
            DropColumn("dbo.User", "CreatedDate");
            DropColumn("dbo.User", "UpdatedDate");
            DropColumn("dbo.User", "UpdatedByUserId");
            DropColumn("dbo.User", "CreatedByUserId");
            DropColumn("dbo.User", "IsDeleted");
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "FirstName");
            DropColumn("dbo.User", "ActiveOrganizationId");
            DropColumn("dbo.Role", "Description");
            DropTable("dbo.OrganizationRole");
            DropTable("dbo.UserOrganizationRole");
            DropTable("dbo.Organization");
            RenameColumn(table: "dbo.User", name: "UserId", newName: "Id");
            RenameTable(name: "dbo.UserLogin", newName: "AspNetUserLogins");
            RenameTable(name: "dbo.UserClaim", newName: "AspNetUserClaims");
            RenameTable(name: "dbo.User", newName: "AspNetUsers");
            RenameTable(name: "dbo.UserRole", newName: "AspNetUserRoles");
            RenameTable(name: "dbo.Role", newName: "AspNetRoles");
        }
    }
}
