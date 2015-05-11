namespace GeneX.Security.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSchema : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.User", newSchema: "Security");
        }
        
        public override void Down()
        {
            MoveTable(name: "Security.User", newSchema: "dbo");
        }
    }
}
