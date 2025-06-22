namespace server.Models.DTO
{
    public class TicketDtoTheen
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly OrderDate { get; set; }
        public TicketStatus Status { get; set; }
    }
}
