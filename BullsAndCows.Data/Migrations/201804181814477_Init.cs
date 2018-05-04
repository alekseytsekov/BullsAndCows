namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
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
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartAt = c.DateTime(nullable: false),
                        EndAt = c.DateTime(),
                        UserId = c.String(maxLength: 128),
                        UserGuessedNumber = c.Int(nullable: false),
                        AIGuessedNumber = c.Int(nullable: false),
                        Bulls = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GameTurns",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserGuess = c.Int(nullable: false),
                        CommentOnUserGuess = c.String(),
                        AIGuess = c.Int(nullable: false),
                        CommentOnAIGuess = c.String(),
                        DigitToChange = c.Int(nullable: false),
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
            DropForeignKey("dbo.Games", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameTurns", "GameId", "dbo.Games");
            DropIndex("dbo.TopGameRankings", new[] { "GameRankingId" });
            DropIndex("dbo.GameTurns", new[] { "GameId" });
            DropIndex("dbo.Games", new[] { "UserId" });
            DropIndex("dbo.GameRankings", new[] { "GameId" });
            DropTable("dbo.TopGameRankings");
            DropTable("dbo.GameTurns");
            DropTable("dbo.Games");
            DropTable("dbo.GameRankings");
        }
    }
}
