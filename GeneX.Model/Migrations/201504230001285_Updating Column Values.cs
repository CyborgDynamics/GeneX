namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatingColumnValues : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SNP", "ClusterId", c => c.String(nullable: false, maxLength: 255, unicode: false));
            AlterColumn("dbo.SNP", "Genotype", c => c.String(maxLength: 4, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SNP", "Genotype", c => c.String());
            AlterColumn("dbo.SNP", "ClusterId", c => c.String());
        }
    }
}
