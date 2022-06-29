using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerStatResourceServer.DTO.Requests;
using SoccerStatResourceServer.Models;
using SoccerStatResourceServer.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private IRepository<Player> playerRepository;
        private IRepository<Team> teamRepository;
                
        public PlayerController(IRepository<Player> playerRepository, IRepository<Team> teamRepository)
        {
            this.playerRepository = playerRepository;
            this.teamRepository = teamRepository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Player> players = await playerRepository.GetAllAsync();
                return Ok(players);
            }
            catch(Exception ex)
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
                Player player = await playerRepository.GetByIdAsync(id.ToString());
                if (player == null)
                    return NotFound();
                return Ok(player);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] PlayerRequest playerRequest)
        {
            try
            {
                if(playerRequest == null)
                    return BadRequest();

                if (!ModelState.IsValid)
                    return BadRequest(playerRequest);

                Team team = await teamRepository.GetByIdAsync(playerRequest.TeamId.ToString());
                if (team == null)
                    return NotFound("Team not found");

                Player player = new Player()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = playerRequest.Name,
                    TeamId = team.Id,
                };

                await playerRepository.CreateAsync(player);
                await playerRepository.SaveAsync();

                return CreatedAtAction("getbyid", new { id = player.Id }, player);

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove/{id:Guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id is empty");

                Player player = await playerRepository.GetByIdAsync(id.ToString());
                if (player == null)
                    return NotFound();

                playerRepository.Delete(player);
                await playerRepository.SaveAsync();
                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdatePlayerRequest updatePlayerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(updatePlayerRequest);

                Player player = await playerRepository.GetByIdAsync(updatePlayerRequest.PlayerId.ToString());
                if (player == null)
                    return NotFound("Player not found");

                Team team = await teamRepository.GetByIdAsync(updatePlayerRequest.TeamId.ToString());
                if (team == null)
                    return NotFound("Team not found");

                player.Name = updatePlayerRequest.Name;
                player.TeamId = updatePlayerRequest.TeamId.ToString();

                playerRepository.Update(player);
                await playerRepository.SaveAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
