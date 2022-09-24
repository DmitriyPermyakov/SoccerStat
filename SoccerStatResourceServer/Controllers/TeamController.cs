using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoccerStatResourceServer.DTO.Requests;
using SoccerStatResourceServer.DTO.Responses;
using SoccerStatResourceServer.Models;
using SoccerStatResourceServer.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SoccerStatResourceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private IRepository<Team> teamRepository;
        private IRepository<League> leagueRepository;

        public TeamController(IRepository<Team> teamRepository, IRepository<League> leagueRepository)
        {
            this.teamRepository = teamRepository;
            this.leagueRepository = leagueRepository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Team> teams = await teamRepository.GetAllAsync();
                List<TeamResponse> teamResponse = new List<TeamResponse>();
                foreach(Team t in teams)
                {
                    var teamResp = new TeamResponse()
                    {
                        Id = t.Id.ToString(),
                        Name = t.Name,
                        ImageUrl = t.ImageUrl,
                        LeagueId = t.LeagueId.ToString()
                    };
                    teamResponse.Add(teamResp);
                }
                return Ok(teamResponse);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getById/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Empty guid");

                Team team = await teamRepository.GetByIdAsync(id.ToString());
                if (team == null)
                    return NotFound();
                else
                {
                    TeamResponse teamResp = new TeamResponse()
                    {
                        Id = team.Id.ToString(),
                        ImageUrl = team.ImageUrl,
                        Name = team.Name,
                        LeagueId = team.LeagueId
                    };
                    return Ok(teamResp);                
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] TeamRequest teamRequest)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(teamRequest);
                if (teamRequest.LeagueId == Guid.Empty)
                    return BadRequest("League id is empty");

                League league = await leagueRepository.GetByIdAsync(teamRequest.LeagueId.ToString());
                if (league == null)
                    return NotFound("League not found");

                Team newTeam = new Team()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageUrl = teamRequest.ImageUrl,
                    Name = teamRequest.Name,                    
                    LeagueId = league.Id
                };

                await teamRepository.CreateAsync(newTeam);
                await teamRepository.SaveAsync();

                return CreatedAtAction("getbyid", new { id = newTeam.Id }, newTeam);
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

                Team team = await teamRepository.GetByIdAsync(id.ToString());
                if (team == null)
                    return NotFound("Team not found");
                else
                {
                    teamRepository.Delete(team);
                    await teamRepository.SaveAsync();
                    return NoContent();
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] TeamRequest updateTeamRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(updateTeamRequest);
                if (updateTeamRequest.Id == Guid.Empty)
                    return BadRequest("TeamId is empty");
                if(updateTeamRequest.LeagueId == Guid.Empty)
                    return BadRequest(updateTeamRequest);

                Team team = await teamRepository.GetByIdAsync(updateTeamRequest.Id.ToString());
                if (team == null)
                    return NotFound("Team not found");

                League league = await leagueRepository.GetByIdAsync(updateTeamRequest.LeagueId.ToString());
                if (league == null)
                    return NotFound("League not found");

                team.Name = updateTeamRequest.Name;
                team.ImageUrl = updateTeamRequest.ImageUrl;
                team.LeagueId = updateTeamRequest.LeagueId.ToString();
                

                teamRepository.Update(team);
                await teamRepository.SaveAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
