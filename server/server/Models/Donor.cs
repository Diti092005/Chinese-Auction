namespace server.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool ShowMe { get; set; } = true;

        public List<Gift> Gifts { get; set; }
    }
}
