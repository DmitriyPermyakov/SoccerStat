using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SoccerStatAuthenticationServer.DomainObjects;
using SoccerStatAuthenticationServer.DTOs.Requests;
using SoccerStatAuthenticationServer.Exceptions;
using SoccerStatAuthenticationServer.Services.UserService;

namespace SoccerStatAuthenticationServer.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] UserRequest userRequest)
        {
            try
            {
                if (userRequest == null)
                    return BadRequest();
                if (!ModelState.IsValid)
                    return BadRequest(userRequest);

                User user = await userService.CreateAsync(userRequest);
                return CreatedAtAction("getbyid", new { id = user.Id }, user);

            }
            catch (UserExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<User> users = await userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getbyid/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id is empty");

                User user = await userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (UserNotExistsException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getbyemail/{email}")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            try
            {
                if (email == null)
                    return BadRequest("Email is null");

                User user = await userService.GetByEmailAsync(email);

                return Ok(user);
            }
            catch (UserNotExistsException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove/{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id is empty");

                await userService.DeleteAsync(id);
                return NoContent();
            }
            catch (UserNotExistsException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update/{id:Guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UserRequest userRequest)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id is empty");
                if (userRequest == null)
                    return BadRequest(userRequest);

                await userService.UpdateAsync(id, userRequest);

                return NoContent();
            }
            catch(UserNotExistsException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
