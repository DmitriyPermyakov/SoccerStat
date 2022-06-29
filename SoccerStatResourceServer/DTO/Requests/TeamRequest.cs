using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class TeamRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid LeagueId { get; set; } 
    }

    public class UpdateTeamRequest : TeamRequest
    {
        [Required]
        public Guid TeamId { get; set; }
    }
}
