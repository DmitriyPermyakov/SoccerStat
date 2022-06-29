using System.Collections.Generic;

namespace SoccerStatResourceServer.Models
{
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }  
        public string LeagueId { get; set; }
        public virtual League League { get; set; }
        public virtual List<Player> Players { get; set; }              
    }
}
