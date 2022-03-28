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
using SoccerStatAuthenticationServer.AuthenticationSettings;

namespace SoccerStatAuthenticationServer.Services.Authenticator
{
    public class AccountService : IAccountService
    {
        private IUserRepository userRepository;
        private ITokenRepository tokenRepository;
        private ITokenGenerator tokenGenerator;
        private IPasswordHasher passwordHasher;
        private JwtSettings jwtSettings;

        public AccountService(IUserRepository userRepository,
            ITokenRepository tokenRepository,
            ITokenGenerator tokenGenerator,
            IPasswordHasher passwordHasher,
            JwtSettings jwtSettings)
        {
            this.userRepository = userRepository;
            this.tokenGenerator = tokenGenerator;
            this.passwordHasher = passwordHasher;
            this.jwtSettings = jwtSettings;
            this.tokenRepository = tokenRepository;
        }

        public async Task<AuthenticationResult> Register(RegisterRequest registerRequest)
        {
            User user = await userRepository.GetByEmailAsync(registerRequest.Email);
            if (user != null)
                throw new UserExistsException("User already exists");

            string passwordHash = passwordHasher.HashPassword(registerRequest.Password);
            User userToCreate = new()
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                PasswordHash = passwordHash
            };

            User createdUser = await userRepository.CreateAsync(userToCreate);

            string refreshToken = tokenGenerator.GenerateToken(TokenType.RefreshToken);
            string accessToken = tokenGenerator.GenerateToken(TokenType.AccessToken);

            RefreshToken createdRefreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes),
                Invalidated = false,
                Used = false,
                User = createdUser
            };

            _ = await tokenRepository.CreateAsync(createdRefreshToken);

            AuthenticationResult result = new AuthenticationResult()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return result;
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
