namespace GeneX.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveNotesAsRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SNPedia", "Notes", c => c.String(maxLength: 125, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SNPedia", "Notes", c => c.String(nullable: false, maxLength: 125, unicode: false));
        }
    }
}
