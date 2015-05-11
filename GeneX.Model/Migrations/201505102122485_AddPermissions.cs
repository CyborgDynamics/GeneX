namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPermissions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Model.GenomePermission",
                c => new
                    {
                        GenomePermissionId = c.Guid(nullable: false),
                        OrganizationId = c.Guid(nullable: false),
                        GenomeId = c.Guid(nullable: false),
                        PermissionId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.GenomePermissionId)
                .ForeignKey("Model.Genome", t => t.GenomeId, cascadeDelete: true)
                .Index(t => t.GenomeId);
            
            CreateTable(
                "Model.Permission",
                c => new
                    {
                        PermissionId = c.Guid(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.PermissionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Model.GenomePermission", "GenomeId", "Model.Genome");
            DropIndex("Model.GenomePermission", new[] { "GenomeId" });
            DropTable("Model.Permission");
            DropTable("Model.GenomePermission");
        }
    }
}
