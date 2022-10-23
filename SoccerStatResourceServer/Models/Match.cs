using System;
namespace SoccerStatResourceServer.Models
{
    public class Match
    {
        public string Id { get; set; }
        public string LeagueId { get; set; }
        public virtual League League { get; set; }
        public string HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public string AwayTeamId { get; set; }
        public virtual Team AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public int HomeTeamFullTime { get; set; }
        public int AwayTeamFullTime { get; set; }
        public int HomeTeamExtraTime { get; set; }
        public int AwayTeamExtraTime { get; set; }
        public int HomeTeamPenalties { get; set; }
        public int AwayTeamPenalties { get; set; }
    }   

    public enum Status
    {
        SCHEDULED,
        LIVE,
        IN_PLAY,
        PAUSED,
        FINISHED,
        POSTPONED,
        SUSPENDED,
        CANCELED
    }
}
