using System;
using System.ComponentModel.DataAnnotations;
using SoccerStatResourceServer.Models;

namespace SoccerStatResourceServer.DTO.Requests
{
    public class MatchRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid HomeTeamId { get; set; }
        [Required]
        public Guid AwayTeamId { get; set; }
        [Required]
        public Guid LeagueId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public int HomeTeamFullTime { get; set; }
        [Required]
        public int AwayTeamFullTime { get; set; }
        [Required]
        public int HomeTeamExtraTime { get; set; }
        [Required]
        public int AwayTeamExtraTime { get; set; }
        [Required]
        public int HomeTeamPenalties { get; set; }
        [Required]
        public int AwayTeamPenalties { get; set; }

    }
}
