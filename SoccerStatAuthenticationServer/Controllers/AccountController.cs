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
using Microsoft.AspNetCore.Authorization;
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
            if (!ModelState.IsValid)
                return BadRequest(registerRequest);
                        
            try
            {
               _ = await accountService.Register(registerRequest);
            }
            catch(UserExistsException)
            {
                return BadRequest(registerRequest);
            }

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(loginRequest);

            AuthenticationResult result;
            try
            {
                result = await accountService.Login(loginRequest);
            }
            catch(AuthenticationException)
            {
                return Unauthorized(loginRequest);
            }

            return Ok(result);
        }

        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(refreshTokenRequest);

            AuthenticationResult result = null;
            try
            {
                result  = await accountService.RefreshToken(refreshTokenRequest);
            }
            catch(AccessTokenValidationTimeException)
            {
                return Ok(refreshTokenRequest);
            }
            catch(RefreshTokenException ex)
            {
                return Unauthorized(ex.Message);
            }
                     
            
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(refreshTokenRequest);

            Microsoft.EntityFrameworkCore.EntityState state;
            try
            {
                state = await accountService.Logout(refreshTokenRequest);
            }
            catch(RefreshTokenException ex)
            {
                return BadRequest(ex.Message);
            }

            if (state == Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {                
                return Ok(state);
            }

            return BadRequest();

        }
        
    }
}
