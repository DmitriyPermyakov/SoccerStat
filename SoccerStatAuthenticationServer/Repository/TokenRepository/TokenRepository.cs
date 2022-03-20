﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SoccerStatAuthenticationServer.DTOs.DomainObjects;
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
        public async Task<RefreshToken> Create(RefreshToken refreshToken)
        {
            var createdRefreshToken = await context.RefreshTokens.AddAsync(refreshToken);
            _ = await context.SaveChangesAsync();
            return createdRefreshToken.Entity;
        }

        public async Task<RefreshToken> GetById(int id)
        {
            RefreshToken token = await context.RefreshTokens.FindAsync(id);
            return token;
        }

        public async Task<RefreshToken> GetByToken(RefreshToken refreshToken)
        {
            RefreshToken token = await context.RefreshTokens.Where(t => t.Token == refreshToken.Token).FirstOrDefaultAsync();
            return token;
        }
    }
}