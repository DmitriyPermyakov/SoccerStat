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
    public class LeagueController : ControllerBase
    {
        private IRepository<League> repository;
        
        public LeagueController(IRepository<League> repository)
        {
            this.repository = repository;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                List<League> leagues = await repository.GetAllAsync();
                List<LeagueResponse> leagueResponse = new List<LeagueResponse>();
                if(leagues == null)
                    return NotFound();
                foreach(League l in leagues)
                {
                    var leagueResp = new LeagueResponse();
                    leagueResp.Id = l.Id;
                    leagueResp.Name = l.Name;
                    leagueResp.ImageUrl = l.ImageUrl;
                    leagueResp.Country = l.Country;

                    leagueResponse.Add(leagueResp);
                }
                return Ok(leagueResponse);
            }
            catch(Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getbyid/{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                    return BadRequest("Empty guid");

                League league = await repository.GetByIdAsync(id.ToString());
                if (league == null)
                    return NotFound();                              
                return Ok(league);               
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("create"), DisableRequestSizeLimit]
        public async Task<IActionResult> CreateAsync([FromBody] LeagueRequest leagueRequest)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest();                

                League newLeague = new League()
                {
                    Id = Guid.NewGuid().ToString(),
                    ImageUrl = leagueRequest.ImageUrl.ToString(),
                    Name = leagueRequest.Name,
                    Country = leagueRequest.Country
                };                
                
                await repository.CreateAsync(newLeague);
                                  
                await repository.SaveAsync();
                return CreatedAtAction("getbyid", new { id = newLeague.Id }, newLeague);
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
                    return BadRequest("Empty guid");

                League league = await repository.GetByIdAsync(id.ToString());
                if (league == null)
                    return NotFound();

                repository.Delete(league);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] LeagueRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest();
                if(!ModelState.IsValid)
                    return BadRequest(request);

                League league = await repository.GetByIdAsync(request.Id.ToString());
                if (league == null)
                    return NotFound("League not found");

                league.Name = request.Name;
                league.ImageUrl = request.ImageUrl;
                league.Country = request.Country;

                repository.Update(league);
                await repository.SaveAsync();
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
