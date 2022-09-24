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
            try
            {
                if (refreshTokenRequest == null)
                    return BadRequest(refreshTokenRequest);
                if (!ModelState.IsValid)
                    return BadRequest(refreshTokenRequest);

                _ = await accountService.RefreshToken(refreshTokenRequest);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}
