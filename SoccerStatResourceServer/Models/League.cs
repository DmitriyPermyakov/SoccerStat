﻿using System.Collections.Generic;

namespace SoccerStatResourceServer.Models
{
    public class League
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual List<Team> Teams { get; set; }   
        public virtual List<Match> Matches { get; set; }
    }
}
