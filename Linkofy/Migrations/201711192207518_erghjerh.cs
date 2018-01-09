namespace Linkofy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class erghjerh : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Data", newName: "DataBs");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DataBs", newName: "Data");
        }
    }
}
