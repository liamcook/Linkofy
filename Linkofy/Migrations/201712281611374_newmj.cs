namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmj : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MJTopics", "topicalTF", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MJTopics", "topicalTF", c => c.Int());
        }
    }
}
