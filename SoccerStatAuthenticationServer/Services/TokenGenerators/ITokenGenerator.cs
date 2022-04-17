using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services.TokenGenerators
{
    public interface ITokenGenerator
    {
        public string GenerateToken(TokenType tokenType);
        public SecurityToken ValidateToken(string token, TokenValidationParameters tokenValidationParameters);       
    }
}
