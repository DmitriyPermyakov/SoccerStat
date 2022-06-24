namespace SoccerStatResourceServer.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public League League { get; set; }
        public Team Team { get; set; }
    }
}
