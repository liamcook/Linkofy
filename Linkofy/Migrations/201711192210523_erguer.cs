namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class erguer : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DataBs", newName: "Data");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Data", newName: "DataBs");
        }
    }
}
