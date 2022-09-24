using Microsoft.IdentityModel.Tokens;
using SoccerStatAuthenticationServer.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services.TokenGenerators
{
    public interface ITokenGenerator
    {
        public Task<string> GenerateToken(TokenType tokenType, User user);
        public SecurityToken ValidateToken(string token, TokenValidationParameters tokenValidationParameters);       
    }
}
