namespace server.Models.DTO
{
    public class GiftDtoTheen
    {
        public int Id { get; set; }
        public string GiftName { get; set; }
        public decimal Price { get; set; }
        public string? Details { get; set; }
        public string? ImageUrl { get; set; }
        public UserDtoTheen? Winner { get; set; }
    }
}
