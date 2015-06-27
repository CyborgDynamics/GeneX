namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "Reference.SNPedia", newName: "Permission");
            MoveTable(name: "Reference.Permission", newSchema: "Model");
            MoveTable(name: "dbo.SNPedia", newSchema: "Reference");
        }
        
        public override void Down()
        {
            MoveTable(name: "Reference.SNPedia", newSchema: "dbo");
            MoveTable(name: "Model.Permission", newSchema: "Reference");
            RenameTable(name: "Reference.Permission", newName: "SNPedia");
        }
    }
}
