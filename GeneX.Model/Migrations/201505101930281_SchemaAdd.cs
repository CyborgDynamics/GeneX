namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchemaAdd : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Genome", newSchema: "Model");
            MoveTable(name: "dbo.SNP", newSchema: "Model");
        }
        
        public override void Down()
        {
            MoveTable(name: "Model.SNP", newSchema: "dbo");
            MoveTable(name: "Model.Genome", newSchema: "dbo");
        }
    }
}
