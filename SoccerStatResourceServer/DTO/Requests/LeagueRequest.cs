using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class LeagueRequest
    {
        [Required]       
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        [Required]        
        public string Name { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
