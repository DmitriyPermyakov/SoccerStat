namespace SoccerStatResourceServer.Models
{
    public class Score
    {
        public string Id { get; set; }
        public int HomeTeamFullTime { get; set; }
        public int AwayTeamFullTime { get; set; }
        public int HomeTeamExtraTime { get; set; }
        public int AwayTeamExtraTime { get; set; }
        public int HomeTeamPenalties { get; set; }
        public int AwayTeamPenalties { get; set; }
    }
}
