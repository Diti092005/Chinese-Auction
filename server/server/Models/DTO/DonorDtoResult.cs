namespace server.Models.DTO
{
    public class DonorDtoResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool ShowMe { get; set; } = true;

        public List<GiftDtoTheen> Gifts { get; set; }
    }
}
