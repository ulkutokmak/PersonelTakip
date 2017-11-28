namespace PT.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "DepertmentID", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepertmentID" });
            AlterColumn("dbo.AspNetUsers", "DepertmentID", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "DepertmentID");
            AddForeignKey("dbo.AspNetUsers", "DepertmentID", "dbo.Departments", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DepertmentID", "dbo.Departments");
            DropIndex("dbo.AspNetUsers", new[] { "DepertmentID" });
            AlterColumn("dbo.AspNetUsers", "DepertmentID", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DepertmentID");
            AddForeignKey("dbo.AspNetUsers", "DepertmentID", "dbo.Departments", "ID", cascadeDelete: true);
        }
    }
}
