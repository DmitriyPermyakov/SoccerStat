using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SoccerStatAuthenticationServer.AuthenticationSettings;

namespace SoccerStatAuthenticationServer.Services.ValidationParameters
{
    public class ValidationParametersFactory
    {
        private readonly JwtSettings jwtSettings;
        public ValidationParametersFactory(JwtSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
        }
        public TokenValidationParameters RefreshTokenValidationParameters
        {
            get
            {
                return new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.RefreshTokenSecret))
                };
            }            
        }
        public TokenValidationParameters AccessTokenValidationParameters
        {
            get
            {
                return new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.AccessTokenSecret))
                };
            }
        }
    }
}
