namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updategameadduser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Games", "UserId");
            AddForeignKey("dbo.Games", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Games", new[] { "UserId" });
            AlterColumn("dbo.Games", "UserId", c => c.String());
        }
    }
}
