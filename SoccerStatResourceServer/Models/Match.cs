using System;
namespace SoccerStatResourceServer.Models
{
    public class Match
    {
        public string Id { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTime Date { get; set; }
        public Status Status { get; set; }
        public Score Score { get; set; }
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
