namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAllele : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SNP", "Allele1", c => c.String(maxLength: 1, unicode: false));
            AddColumn("dbo.SNP", "Allele2", c => c.String(maxLength: 1, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SNP", "Allele2");
            DropColumn("dbo.SNP", "Allele1");
        }
    }
}
