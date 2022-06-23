using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.Repository.TokenRepository;

namespace SoccerStatAuthenticationServer.Repository.TokenRepository
{
    public class TokenRepository : ITokenRepository
    {
        private AuthenticationServerDbContext context;
        public TokenRepository(AuthenticationServerDbContext ctx)
        {
            context = ctx;
        }
        public async Task<RefreshToken> CreateAsync(RefreshToken refreshToken)
        {
            var createdRefreshToken = await context.RefreshTokens.AddAsync(refreshToken);
            _ = await context.SaveChangesAsync();
            return createdRefreshToken.Entity;
        }

        public async Task<RefreshToken> GetByIdAsync(int id)
        {
            RefreshToken token = await context.RefreshTokens.FindAsync(id);
            return token;
        }

        public async Task<RefreshToken> GetByTokenAsync(string refreshToken)
        {
            RefreshToken token = await context.RefreshTokens.Where(t => t.Token == refreshToken).FirstOrDefaultAsync();
            return token;
        }

        public async Task<RefreshToken> GetByUserIdAsync(Guid userId)
        {
            RefreshToken token = await context.RefreshTokens.Where(t => t.UserId == userId).FirstOrDefaultAsync();
            return token;
        }
    }
}
