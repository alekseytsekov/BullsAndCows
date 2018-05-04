namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRanking : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GameRankings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TimeSpanSeconds = c.Long(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: false)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.TopGameRankings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameRankingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameRankings", t => t.GameRankingId, cascadeDelete: false)
                .Index(t => t.GameRankingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TopGameRankings", "GameRankingId", "dbo.GameRankings");
            DropForeignKey("dbo.GameRankings", "GameId", "dbo.Games");
            DropIndex("dbo.TopGameRankings", new[] { "GameRankingId" });
            DropIndex("dbo.GameRankings", new[] { "GameId" });
            DropTable("dbo.TopGameRankings");
            DropTable("dbo.GameRankings");
        }
    }
}
