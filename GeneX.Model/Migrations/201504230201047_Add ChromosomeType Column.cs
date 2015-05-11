namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChromosomeTypeColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SNP", "ChromosomeType", c => c.String(nullable: false, maxLength: 8, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SNP", "ChromosomeType");
        }
    }
}
