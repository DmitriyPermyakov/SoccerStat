using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class LeagueRequest
    {
        [Required]       
        public Guid Id { get; set; }
        [Required]        
        public string Name { get; set; }
    }
}
