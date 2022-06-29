using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
    public class MatchController : ControllerBase
    {
        private IRepository<Match> matchRepository;
        private IRepository<Team> teamRepository;

        public MatchController(IRepository<Match> matchRepository, IRepository<Team> teamRepository)
        {
            this.matchRepository = matchRepository;
            this.teamRepository = teamRepository;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Match> matches = await matchRepository.GetAllAsync();
                return Ok(matches);
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
                    return BadRequest("Match id is empty");
                Match match = await matchRepository.GetByIdAsync(id.ToString());
                if (match == null)
                    return NotFound("Match not found");

                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove/{id:Guid}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Match id is empty");

                Match match = await matchRepository.GetByIdAsync(id.ToString());
                if (match == null)
                    return NotFound("Match not found");

                matchRepository.Delete(match);
                await matchRepository.SaveAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] MatchRequest matchRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(matchRequest);

                if (matchRequest.HomeTeamId == Guid.Empty || matchRequest.AwayTeamId == Guid.Empty)
                    return BadRequest("Team not found");
                Team homeTeam = await teamRepository.GetByIdAsync(matchRequest.HomeTeamId.ToString());
                if (homeTeam == null)
                    return NotFound("Team not found");
                Team awayTeam = await teamRepository.GetByIdAsync(matchRequest.AwayTeamId.ToString());
                if (awayTeam == null)
                    return NotFound("Team not found");

                Match match = new Match()
                {
                    Id = Guid.NewGuid().ToString(),
                    HomeTeamId = homeTeam.Id,
                    AwayTeamId = awayTeam.Id,
                    Date = matchRequest.Date,
                    Status = matchRequest.Status,
                    HomeTeamFullTime = matchRequest.HomeTeamFullTime,
                    AwayTeamFullTime = matchRequest.AwayTeamFullTime,
                    HomeTeamExtraTime = matchRequest.HomeTeamExtraTime,
                    AwayTeamExtraTime = matchRequest.AwayTeamExtraTime,
                    HomeTeamPenalties = matchRequest.HomeTeamPenalties,
                    AwayTeamPenalties = matchRequest.AwayTeamPenalties
                };

                await matchRepository.CreateAsync(match);
                await matchRepository.SaveAsync();

                return CreatedAtAction("getbyid", new { id = match.Id }, match);
                

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateAsync([FromBody]UpdateMatchRequest updateMatchRequest)
        {
            try
            {
                if (updateMatchRequest == null)
                    return BadRequest("Updated object is null");

                if (!ModelState.IsValid)
                    return BadRequest(updateMatchRequest);

                Match match = await matchRepository.GetByIdAsync(updateMatchRequest.Id.ToString());
                if (match == null)
                    return NotFound("Match not found");

                match.HomeTeamId = updateMatchRequest.HomeTeamId.ToString();
                match.AwayTeamId = updateMatchRequest.AwayTeamId.ToString();
                match.Date = updateMatchRequest.Date;
                match.Status = updateMatchRequest.Status;
                match.HomeTeamFullTime = updateMatchRequest.HomeTeamFullTime;
                match.AwayTeamFullTime = updateMatchRequest.AwayTeamFullTime;
                match.HomeTeamExtraTime = updateMatchRequest.HomeTeamExtraTime;
                match.AwayTeamExtraTime = updateMatchRequest.AwayTeamExtraTime;
                match.HomeTeamPenalties = updateMatchRequest.HomeTeamPenalties;
                match.AwayTeamPenalties = updateMatchRequest.AwayTeamPenalties;

                matchRepository.Update(match);
                await matchRepository.SaveAsync();

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("patch/{id:Guid}")]
        public async Task<IActionResult> PatchAsync(Guid id, [FromBody] JsonPatchDocument<Match> entityToPatch)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Id is empty");
                Match match = await matchRepository.GetByIdAsync(id.ToString());
                if(match == null)
                    return NotFound();

                entityToPatch.ApplyTo(match);
                matchRepository.Update(match);
                await matchRepository.SaveAsync();

                return NoContent();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
