using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SoccerStatResourceServer.DTO.Requests;
using SoccerStatResourceServer.DTO.Responses;
using SoccerStatResourceServer.Exceptions;
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
        private IRepository<League> leagueRepository;

        public MatchController(IRepository<Match> matchRepository, IRepository<Team> teamRepository, IRepository<League> leagueRepository)
        {
            this.matchRepository = matchRepository;
            this.teamRepository = teamRepository;
            this.leagueRepository = leagueRepository;
        }

        [HttpGet("getMatchesByLeagueId/{id:Guid}")]
        public async Task<IActionResult> GetMatchesByLeagueIdAsync(Guid id)
        {
            if(id == Guid.Empty)
                return NotFound("League not found");
            try
            {
                List<Match> matches = await matchRepository.GetAllAsync();

                List<Match> filteredMatches = matches.FindAll(m => m.LeagueId == id.ToString());

                if(filteredMatches.Count == 0)
                {
                    return NotFound("Matches not found");
                }

                List<MatchResponse> matchResponses = new List<MatchResponse>();
                foreach (Match match in filteredMatches)
                {
                    matchResponses.Add(new MatchResponse()
                    {
                        Id = match.Id,
                        League = match.League,
                        HomeTeam = match.HomeTeam,
                        AwayTeam = match.AwayTeam,
                        Date = match.Date,
                        Status = match.Status,
                        HomeTeamFullTime = match.HomeTeamFullTime,
                        AwayTeamFullTime = match.AwayTeamFullTime,
                        HomeTeamExtraTime = match.HomeTeamExtraTime,
                        AwayTeamExtraTime = match.AwayTeamExtraTime,
                        HomeTeamPenalties = match.HomeTeamPenalties,
                        AwayTeamPenalties = match.AwayTeamPenalties
                    });
                }
                return Ok(matchResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getMatchesByTeamId/{id:Guid}")]
        public async Task<ActionResult> GetMatchByTeamId(Guid id)
        {            
            Console.WriteLine("**********" + id.ToString());
            if (id == Guid.Empty)
            {
                return NotFound("Team not found");

            }
            try
            {
                List<Match> matches = await matchRepository.GetAllAsync();

                List<Match> filteredMatches = matches.FindAll(m => m.HomeTeamId == id.ToString());

                if (filteredMatches.Count == 0)
                {
                    return NotFound("Matches not found");
                }

                List<MatchResponse> matchResponses = new List<MatchResponse>();
                foreach (Match match in filteredMatches)
                {
                    matchResponses.Add(new MatchResponse()
                    {
                        Id = match.Id,
                        League = match.League,
                        HomeTeam = match.HomeTeam,
                        AwayTeam = match.AwayTeam,
                        Date = match.Date,
                        Status = match.Status,
                        HomeTeamFullTime = match.HomeTeamFullTime,
                        AwayTeamFullTime = match.AwayTeamFullTime,
                        HomeTeamExtraTime = match.HomeTeamExtraTime,
                        AwayTeamExtraTime = match.AwayTeamExtraTime,
                        HomeTeamPenalties = match.HomeTeamPenalties,
                        AwayTeamPenalties = match.AwayTeamPenalties
                    });
                }
                return Ok(matchResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<Match> matches = await matchRepository.GetAllAsync();
                
                List<MatchResponse> matchResponses = new List<MatchResponse>();
                foreach (Match match in matches)
                {
                    matchResponses.Add(new MatchResponse()
                    {
                        Id = match.Id,
                        League = match.League,
                        HomeTeam = match.HomeTeam,
                        AwayTeam = match.AwayTeam,
                        Date = match.Date,
                        Status = match.Status,
                        HomeTeamFullTime = match.HomeTeamFullTime,
                        AwayTeamFullTime = match.AwayTeamFullTime,
                        HomeTeamExtraTime = match.HomeTeamExtraTime,
                        AwayTeamExtraTime = match.AwayTeamExtraTime,
                        HomeTeamPenalties = match.HomeTeamPenalties,
                        AwayTeamPenalties = match.AwayTeamPenalties
                    });
                }
                return Ok(matchResponses);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getbyid")]
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

        [HttpDelete("remove")]
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

                await ValidateRequest(matchRequest);               

                Match match = new Match()
                {
                    Id = Guid.NewGuid().ToString(),
                    LeagueId = matchRequest.LeagueId.ToString(),
                    HomeTeamId = matchRequest.HomeTeamId.ToString(),
                    AwayTeamId = matchRequest.AwayTeamId.ToString(),
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
            catch(ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateAsync([FromBody]MatchRequest updateMatchRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(updateMatchRequest);

                Match match = await matchRepository.GetByIdAsync(updateMatchRequest.Id.ToString());
                if (match == null)
                    return NotFound("Match not found");

                await ValidateRequest(updateMatchRequest);

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
            catch(ValidationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("patch")]
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

        private async Task<bool> ValidateRequest(MatchRequest matchRequest)
        {
            if (matchRequest.HomeTeamId == Guid.Empty
                    || matchRequest.AwayTeamId == Guid.Empty
                    || matchRequest.LeagueId == Guid.Empty)
                throw new ValidationException("Guid is empty");

            League league = await leagueRepository.GetByIdAsync(matchRequest.LeagueId.ToString());
            if (league == null)
                throw new ValidationException("League not found");

            Team homeTeam = await teamRepository.GetByIdAsync(matchRequest.HomeTeamId.ToString());
            if (homeTeam == null)
                throw new ValidationException("Home team not found");

            Team awayTeam = await teamRepository.GetByIdAsync(matchRequest.AwayTeamId.ToString());
            if (awayTeam == null)
                throw new ValidationException("Away team not found");

            return true;
        }
    }
}
