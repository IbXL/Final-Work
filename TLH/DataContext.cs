using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TLH.Entity;
using TLH.Models.Matches;

namespace TLH
{
    public class DataContext : IdentityDbContext<Users>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<PT> PTs { get; set; }

        public DbSet<TT> TTs { get; set; }

        public DbSet<DayOfT> DayOfTs { get; set; }

        public DbSet<Match> Matchs { get; set; }

        public DbSet<KScore> KScores { get; set; }

        public DbSet<PScore> PScores { get; set; }

        public DbSet<ScoreSystem> ScoreSystems { get; set; }

        public DbSet<Prize> Prizes { get; set; }

        public DbSet<TeamAchivement> TeamAchivements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
