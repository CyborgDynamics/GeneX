namespace GeneX.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitializeAgain : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.Organization",
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
                .ForeignKey("Security.User", t => t.CreatedByUserId)
                .ForeignKey("Security.User", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "Security.User",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        ActiveOrganizationId = c.Guid(),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedByUserId = c.Guid(),
                        UpdatedByUserId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("Security.Organization", t => t.ActiveOrganizationId)
                .ForeignKey("Security.User", t => t.CreatedByUserId)
                .ForeignKey("Security.User", t => t.UpdatedByUserId)
                .Index(t => t.ActiveOrganizationId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Security.UserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.UserLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.UserRole",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Security.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Security.OrganizationRole",
                c => new
                    {
                        OrganizationRoleId = c.Guid(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedByUserId = c.Guid(),
                        UpdatedByUserId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrganizationRoleId)
                .ForeignKey("Security.User", t => t.CreatedByUserId)
                .ForeignKey("Security.Organization", t => t.OrganizationId, cascadeDelete: true)
                .ForeignKey("Security.User", t => t.UpdatedByUserId)
                .Index(t => t.OrganizationId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            CreateTable(
                "Security.OrganizationRoleItem",
                c => new
                    {
                        OrganizationRoleItemId = c.Guid(nullable: false),
                        OrganizationRoleId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedByUserId = c.Guid(),
                        UpdatedByUserId = c.Guid(),
                        UpdatedDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy_Id = c.Guid(),
                        UpdatedBy_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.OrganizationRoleItemId)
                .ForeignKey("Security.User", t => t.CreatedBy_Id)
                .ForeignKey("Security.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("Security.User", t => t.UpdatedBy_Id)
                .ForeignKey("Security.OrganizationRole", t => t.OrganizationRoleId, cascadeDelete: true)
                .Index(t => t.OrganizationRoleId)
                .Index(t => t.RoleId)
                .Index(t => t.CreatedBy_Id)
                .Index(t => t.UpdatedBy_Id);
            
            CreateTable(
                "Security.Role",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "Security.UserOrganizationRole",
                c => new
                    {
                        UserOrganizationRoleId = c.Guid(nullable: false),
                        OrganizationRoleId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.UserOrganizationRoleId)
                .ForeignKey("Security.OrganizationRole", t => t.OrganizationRoleId, cascadeDelete: true)
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.OrganizationRoleId }, unique: true, name: "IX_OrganizationRoleAndUser");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.UserOrganizationRole", "UserId", "Security.User");
            DropForeignKey("Security.UserOrganizationRole", "OrganizationRoleId", "Security.OrganizationRole");
            DropForeignKey("Security.OrganizationRole", "UpdatedByUserId", "Security.User");
            DropForeignKey("Security.OrganizationRoleItem", "OrganizationRoleId", "Security.OrganizationRole");
            DropForeignKey("Security.OrganizationRoleItem", "UpdatedBy_Id", "Security.User");
            DropForeignKey("Security.OrganizationRoleItem", "RoleId", "Security.Role");
            DropForeignKey("Security.UserRole", "RoleId", "Security.Role");
            DropForeignKey("Security.OrganizationRoleItem", "CreatedBy_Id", "Security.User");
            DropForeignKey("Security.OrganizationRole", "OrganizationId", "Security.Organization");
            DropForeignKey("Security.OrganizationRole", "CreatedByUserId", "Security.User");
            DropForeignKey("Security.Organization", "UpdatedByUserId", "Security.User");
            DropForeignKey("Security.Organization", "CreatedByUserId", "Security.User");
            DropForeignKey("Security.User", "UpdatedByUserId", "Security.User");
            DropForeignKey("Security.UserRole", "UserId", "Security.User");
            DropForeignKey("Security.UserLogin", "UserId", "Security.User");
            DropForeignKey("Security.User", "CreatedByUserId", "Security.User");
            DropForeignKey("Security.UserClaim", "UserId", "Security.User");
            DropForeignKey("Security.User", "ActiveOrganizationId", "Security.Organization");
            DropIndex("Security.UserOrganizationRole", "IX_OrganizationRoleAndUser");
            DropIndex("Security.Role", "RoleNameIndex");
            DropIndex("Security.OrganizationRoleItem", new[] { "UpdatedBy_Id" });
            DropIndex("Security.OrganizationRoleItem", new[] { "CreatedBy_Id" });
            DropIndex("Security.OrganizationRoleItem", new[] { "RoleId" });
            DropIndex("Security.OrganizationRoleItem", new[] { "OrganizationRoleId" });
            DropIndex("Security.OrganizationRole", new[] { "UpdatedByUserId" });
            DropIndex("Security.OrganizationRole", new[] { "CreatedByUserId" });
            DropIndex("Security.OrganizationRole", new[] { "OrganizationId" });
            DropIndex("Security.UserRole", new[] { "RoleId" });
            DropIndex("Security.UserRole", new[] { "UserId" });
            DropIndex("Security.UserLogin", new[] { "UserId" });
            DropIndex("Security.UserClaim", new[] { "UserId" });
            DropIndex("Security.User", "UserNameIndex");
            DropIndex("Security.User", new[] { "UpdatedByUserId" });
            DropIndex("Security.User", new[] { "CreatedByUserId" });
            DropIndex("Security.User", new[] { "ActiveOrganizationId" });
            DropIndex("Security.Organization", new[] { "UpdatedByUserId" });
            DropIndex("Security.Organization", new[] { "CreatedByUserId" });
            DropTable("Security.UserOrganizationRole");
            DropTable("Security.Role");
            DropTable("Security.OrganizationRoleItem");
            DropTable("Security.OrganizationRole");
            DropTable("Security.UserRole");
            DropTable("Security.UserLogin");
            DropTable("Security.UserClaim");
            DropTable("Security.User");
            DropTable("Security.Organization");
        }
    }
}
