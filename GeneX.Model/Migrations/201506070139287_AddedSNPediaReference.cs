namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSNPediaReference : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Model.Permission", newName: "SNPedia");
            MoveTable(name: "Model.SNPedia", newSchema: "Reference");
            CreateTable(
                "dbo.SNPedia",
                c => new
                    {
                        SNPediaId = c.Guid(nullable: false),
                        ClusterId = c.String(nullable: false, maxLength: 255, unicode: false),
                        Source = c.String(nullable: false, maxLength: 10, unicode: false),
                        ChromosomeType = c.String(nullable: false, maxLength: 8, unicode: false),
                        Chromosome = c.Int(nullable: false),
                        ChipType = c.String(nullable: false, maxLength: 10, unicode: false),
                        Position = c.Int(nullable: false),
                        Notes = c.String(nullable: false, maxLength: 125, unicode: false),
                    })
                .PrimaryKey(t => t.SNPediaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SNPedia");
            MoveTable(name: "Reference.SNPedia", newSchema: "Model");
            RenameTable(name: "Model.SNPedia", newName: "Permission");
        }
    }
}
