namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Updategameaddbullsprop : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "Bulls", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "Bulls");
        }
    }
}
