using SoccerStatAuthenticationServer.DTOs.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Repository.TokenRepository
{
    public interface ITokenRepository
    {
        public Task<RefreshToken> Create(RefreshToken refreshToken);
        public Task<RefreshToken> GetById(int id);
        public Task<RefreshToken> GetByToken(RefreshToken refreshToken);
    }
}
