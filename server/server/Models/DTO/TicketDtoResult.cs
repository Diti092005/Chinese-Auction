namespace server.Models.DTO
{
    public class TicketDtoResult
    {
        public int Id { get; set; }
        public UserDtoTheen User { get; set; }

        public GiftDtoTheen Gift { get; set; }

        public DateOnly OrderDate { get; set; }
        public TicketStatus Status { get; set; }
    }
}
