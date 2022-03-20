using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoccerStatAuthenticationServer.Services
{
    public class AccountService : IAccountService
    {
        public Task<AuthenticationResult> Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            throw new NotImplementedException();
        }

        public Task<AuthenticationResult> Register(RegisterRequest registerRequest)
        {
            throw new NotImplementedException();
        }
    }
}
