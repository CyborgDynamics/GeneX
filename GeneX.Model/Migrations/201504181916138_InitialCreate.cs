namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Genome",
                c => new
                    {
                        GenomeId = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.GenomeId);
            
            CreateTable(
                "dbo.SNP",
                c => new
                    {
                        SNPId = c.Guid(nullable: false),
                        GenomeId = c.Guid(nullable: false),
                        ClusterId = c.String(),
                        Chromosome = c.Int(nullable: false),
                        Position = c.Int(nullable: false),
                        Genotype = c.String(),
                    })
                .PrimaryKey(t => t.SNPId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SNP");
            DropTable("dbo.Genome");
        }
    }
}
