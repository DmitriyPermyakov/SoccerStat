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

        public TokenGenerator(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }

        public string GenerateToken(TokenType tokenType)
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


            //List<Claim> claims = new List<Claim>()
            //{
            //    new Claim(JwtRegisteredClaimNames.Email,  ),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            //};

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,                
                claims: null,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(expirationTime),                
                signingCredentials);

            var createdToken = new JwtSecurityTokenHandler().WriteToken(token);
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
