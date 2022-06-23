using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.Converters;
using SoccerStatAuthenticationServer.DomainObjects;

namespace SoccerStatAuthenticationServer.Repository
{
    public class AuthenticationServerDbContext : DbContext
    {
        public AuthenticationServerDbContext(DbContextOptions<AuthenticationServerDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set;}
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var conterter = new EnumCollectionJsonValueConverter<Role>();
            var comparer = new CollectionValueComparer<Role>();

            modelBuilder.Entity<User>()
                .Property(user => user.Roles)
                .HasConversion(conterter)
                .Metadata.SetValueComparer(comparer);
        }
       
    }

}
