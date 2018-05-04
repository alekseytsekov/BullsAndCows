namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateGameRemoveCorrectIncorrectprops : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Games", "CorrectDigits");
            DropColumn("dbo.Games", "IncorrectDigits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Games", "IncorrectDigits", c => c.String());
            AddColumn("dbo.Games", "CorrectDigits", c => c.String());
        }
    }
}
