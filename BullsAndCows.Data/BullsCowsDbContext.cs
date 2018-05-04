using System.Data.Entity;
using BullsAndCows.Models.Domain;
using BullsAndCows.Models.Domain.Game;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BullsAndCows.Data
{

    public class BullsCowsDbContext : IdentityDbContext<ApplicationUser>
    {
        public BullsCowsDbContext()
            : base("name=BullsCowsDbContext")
        {
        }
        
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameTurn> GameTurns { get; set; }
        public virtual DbSet<GameRanking> GameRanking { get; set; }
        public virtual DbSet<TopGameRanking> TopGameRankings { get; set; }

        public static BullsCowsDbContext Create()
        {
            return new BullsCowsDbContext();
        }
    }
}