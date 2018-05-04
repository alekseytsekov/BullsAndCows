namespace BullsAndCows.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartAt = c.DateTime(nullable: false),
                        EndAt = c.DateTime(),
                        UserId = c.String(),
                        UserGuessedNumber = c.Int(nullable: false),
                        AIGuessedNumber = c.Int(nullable: false),
                        CorrectDigits = c.String(),
                        IncorrectDigits = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GameTurns", "GameId", "dbo.Games");
            DropIndex("dbo.GameTurns", new[] { "GameId" });
            DropTable("dbo.GameTurns");
            DropTable("dbo.Games");
        }
    }
}
