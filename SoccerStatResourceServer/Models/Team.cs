using System.Collections.Generic;

namespace SoccerStatResourceServer.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public League League { get; set; }
        public List<Player> Players { get; set; }
        public List<Match> Matches { get; set; }
    }
}
