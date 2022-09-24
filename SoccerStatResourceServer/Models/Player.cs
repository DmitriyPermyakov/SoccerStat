namespace SoccerStatResourceServer.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }  
        public string TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
