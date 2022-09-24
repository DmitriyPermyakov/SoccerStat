using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoccerStatAuthenticationServer.Services;
using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.DTOs.Responses;
using SoccerStatAuthenticationServer.Services.Authenticator;
using SoccerStatAuthenticationServer.Exceptions;
using SoccerStatAuthenticationServer.DomainObjects;

namespace SoccerStatAuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService accountService;
        public AccountController(IAccountService service)
        {
            accountService = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(registerRequest);

                User createdUser = await accountService.Register(registerRequest);

                return Ok(createdUser.Id);
            }
            catch(UserExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }            
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null)
                    return Unauthorized(loginRequest);
                if (!ModelState.IsValid)
                    return Unauthorized(loginRequest);
                AuthenticationResult result = await accountService.Login(loginRequest);

                return Ok(result);
            }
            catch(AuthenticationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            } 
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(refreshTokenRequest);
            AuthenticationResult authenticationResult = null;

            try
            {
                authenticationResult = await accountService.RefreshToken(refreshTokenRequest);
            }
            catch(AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(AccessTokenValidationTimeException ex)
            {
                return BadRequest(ex.Message);
            }            
            
            return Ok(authenticationResult);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(string refreshToken)
        {
            if (refreshToken == null || refreshToken == String.Empty)
                return BadRequest(refreshToken);

            await accountService.Logout(refreshToken);

            return Ok();
        }
        
    }
}
