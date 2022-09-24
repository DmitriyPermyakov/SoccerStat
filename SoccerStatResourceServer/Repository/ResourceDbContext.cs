using System;
using Microsoft.EntityFrameworkCore;

using SoccerStatResourceServer.Models;

namespace SoccerStatResourceServer.Repository
{
    public class ResourceDbContext : DbContext
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }        
        public DbSet<Match> Matches { get; set; }
        public ResourceDbContext(DbContextOptions<ResourceDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .Property(m => m.Status)
                .HasConversion(s => s.ToString(),
                s => (Status)Enum.Parse(typeof(Status), s));
        }
    }

    
}
