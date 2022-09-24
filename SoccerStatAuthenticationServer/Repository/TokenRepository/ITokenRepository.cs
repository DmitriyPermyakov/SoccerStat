using SoccerStatAuthenticationServer.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Repository.TokenRepository
{
    public interface ITokenRepository
    {
        public Task<RefreshToken> CreateAsync(RefreshToken refreshToken);
        public Task<RefreshToken> GetByIdAsync(int id);
        public Task<RefreshToken> GetByTokenAsync(string refreshToken);
        public Task<RefreshToken> GetByUserIdAsync(Guid userId);
        public Task RemoveToken(RefreshToken refreshToken);
    }
}
