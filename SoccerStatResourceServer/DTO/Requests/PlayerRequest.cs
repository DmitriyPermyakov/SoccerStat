using System;
using System.ComponentModel.DataAnnotations;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class PlayerRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid TeamId { get; set; }
    }

    public class UpdatePlayerRequest : PlayerRequest
    {
        [Required]
        public Guid PlayerId { get; set; }
    }
}
