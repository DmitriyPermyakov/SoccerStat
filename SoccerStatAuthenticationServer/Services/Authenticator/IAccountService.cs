using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services.Authenticator
{
    public interface IAccountService
    {
        public Task<AuthenticationResult> Register(RegisterRequest registerRequest);
        public Task<AuthenticationResult> Login(LoginRequest loginRequest);
        public Task<AuthenticationResult> RefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}
