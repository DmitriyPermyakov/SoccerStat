using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.DTOs.DomainObjects;

namespace SoccerStatAuthenticationServer.Repository
{
    public class AuthenticationServerDbContext : DbContext
    {
        public AuthenticationServerDbContext(DbContextOptions<AuthenticationServerDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set;}
    }
}
