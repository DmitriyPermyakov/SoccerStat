using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.DTOs.Responses;
using System;
using System.Threading.Tasks;
using SoccerStatAuthenticationServer.Repository.UserRepository;
using SoccerStatAuthenticationServer.Repository.TokenRepository;
using SoccerStatAuthenticationServer.Exceptions;
using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.Services.TokenGenerators;
using SoccerStatAuthenticationServer.Services.PasswordHasher;
using SoccerStatAuthenticationServer.AuthenticationSettings;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SoccerStatAuthenticationServer.Services.ValidationParameters;

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

        public async Task<User> Register(RegisterRequest registerRequest)
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
            

            return createdUser;
        }
        public async Task<AuthenticationResult> Login(LoginRequest loginRequest)
        {
            User user = await userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
                throw new AuthenticationException("Invalid user/password");

            bool isPasswordCorrect = passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);
            if (!isPasswordCorrect)
                throw new AuthenticationException("Invalid user/password");

            RefreshToken oldRefreshToken = await tokenRepository.GetByUserIdAsync(user.Id);

            if(oldRefreshToken != null)
            {
                _ = await tokenRepository.RemoveToken(oldRefreshToken);
            }

            string accessToken = tokenGenerator.GenerateToken(TokenType.AccessToken);
            string refreshToken = tokenGenerator.GenerateToken(TokenType.RefreshToken);

            RefreshToken createdRefreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                Token = refreshToken,
                CreationDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddMinutes(jwtSettings.RefreshTokenExpirationMinutes),
                Invalidated = false,
                Used = false,
                User = user
            };

            _ = await tokenRepository.CreateAsync(createdRefreshToken);                   

            AuthenticationResult authResult = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return authResult;
        }

        public async Task<AuthenticationResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedAccessToken = null;
            SecurityToken validatedRefreshToken = null;

            ValidationParametersFactory validationFactory = new ValidationParametersFactory(jwtSettings);

            TokenValidationParameters accessTokenValidationParameters = validationFactory.AccessTokenValidationParameters;
            TokenValidationParameters refreshTokenValidationParameters = validationFactory.RefreshTokenValidationParameters;

            validatedAccessToken = tokenGenerator.ValidateToken(refreshTokenRequest.AccessToken, accessTokenValidationParameters);
            validatedRefreshToken = tokenGenerator.ValidateToken(refreshTokenRequest.RefreshToken, refreshTokenValidationParameters);

            if (validatedAccessToken == null || validatedRefreshToken == null)
                throw new AuthenticationException("Invalid token");            

            if(validatedAccessToken.ValidTo > DateTime.UtcNow)
            {
                throw new AccessTokenValidationTimeException("Access token is valid yet");
            }            

            RefreshToken refreshToken = await tokenRepository.GetByTokenAsync(refreshTokenRequest.RefreshToken);
            if (refreshToken == null || refreshToken.Used || refreshToken.Invalidated || validatedRefreshToken.ValidTo < DateTime.UtcNow)
                throw new RefreshTokenException("Refresh token is invalid");

            _ = await tokenRepository.RemoveToken(refreshToken);

            string newRefreshToken = tokenGenerator.GenerateToken(TokenType.RefreshToken);
            string newAccessToken = tokenGenerator.GenerateToken(TokenType.AccessToken);

            return new AuthenticationResult()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<Microsoft.EntityFrameworkCore.EntityState> Logout(RefreshTokenRequest refreshTokenRequest)
        {
            RefreshToken refreshToken = await tokenRepository.GetByTokenAsync(refreshTokenRequest.RefreshToken);
            if (refreshToken == null)
                throw new RefreshTokenException("Token not found");

            var result = await tokenRepository.RemoveToken(refreshToken);
            return result;
        }

        
    }
}
