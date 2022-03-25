using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoccerStatAuthenticationServer.Repository.UserRepository;
using SoccerStatAuthenticationServer.Repository.TokenRepository;
using SoccerStatAuthenticationServer.Exceptions;
using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.Services.TokenGenerators;
using SoccerStatAuthenticationServer.Services.PasswordHasher;

namespace SoccerStatAuthenticationServer.Services.Authenticator
{
    public class AccountService : IAccountService
    {
        private IUserRepository userRepository;
        private ITokenGenerator tokenGenerator;
        private IPasswordHasher passwordHasher;

        public AccountService(IUserRepository userRepository, ITokenGenerator tokenGenerator, IPasswordHasher passwordHasher)
        {
            this.userRepository = userRepository;
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
        }

        public async Task<AuthenticationResult> Register(RegisterRequest registerRequest)
        {
            User user = await userRepository.GetByEmailAsync(registerRequest.Email);
            if (user != null)
                throw new UserExistsException("User already exists");



                
        }
        public async Task<AuthenticationResult> Login(LoginRequest loginRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthenticationResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            throw new NotImplementedException();
        }

    }
}
