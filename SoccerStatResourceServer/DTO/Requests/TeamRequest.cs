using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class TeamRequest
    {

        [Required]
        public Guid Id { get; set; }        
        public string ImageUrl { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid LeagueId { get; set; } 
    }
}
