using Microsoft.IdentityModel.Tokens;
using SoccerStatAuthenticationServer.AuthenticationSettings;
using SoccerStatAuthenticationServer.DomainObjects;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using SoccerStatAuthenticationServer.Repository.TokenRepository;

namespace SoccerStatAuthenticationServer.Services.TokenGenerators
{
    public enum TokenType
    {
        AccessToken,
        RefreshToken
    }
    public class TokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings jwtSettings;
        private readonly ITokenRepository tokenRepository;

        public TokenGenerator(JwtSettings jwtSettings, ITokenRepository repo)
        {
            this.jwtSettings = jwtSettings;
            this.tokenRepository = repo;
        }
        
        public async Task<string> GenerateToken(TokenType tokenType, User user)
        {
            string tokenSecret = null;
            double expirationTime = 0;
            switch (tokenType)
            {
                case TokenType.AccessToken:
                    tokenSecret = jwtSettings.AccessTokenSecret;
                    expirationTime = jwtSettings.AccessTokenExpirationMinutes;
                    break;
                case TokenType.RefreshToken:
                    tokenSecret = jwtSettings.RefreshTokenSecret;
                    expirationTime = jwtSettings.RefreshTokenExpirationMinutes;
                    break;
            }

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            foreach(var role in user.Roles)
            {
                claims.Add(new Claim("role", role.ToString()));
            }

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,                
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expirationTime),                
                signingCredentials);

            var createdToken = new JwtSecurityTokenHandler().WriteToken(token);
            
            if (tokenType == TokenType.RefreshToken)
            {
                RefreshToken refreshToken = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    Token = createdToken,
                    UserId = user.Id
                };
                await tokenRepository.CreateAsync(refreshToken);
            }
            return createdToken;
        }

        public SecurityToken ValidateToken(string token, TokenValidationParameters tokenValidationParameters)
        {
            var tokenSecurityHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
               ClaimsPrincipal claims = tokenSecurityHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch
            {
                validatedToken = null;
            }

            return validatedToken;
        }
    }
}
