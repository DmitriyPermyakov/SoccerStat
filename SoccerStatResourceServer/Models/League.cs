using System.Collections.Generic;

namespace SoccerStatResourceServer.Models
{
    public class League
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }   
        public List<Match> Matches { get; set; }
    }
}
