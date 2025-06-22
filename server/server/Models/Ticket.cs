namespace server.Models
{
    public enum TicketStatus
    {
        Pending,
        Paid,
        Win
    }
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public int GiftId { get; set; }
        public Gift Gift { get; set; }

        public DateOnly OrderDate { get; set; }
        public TicketStatus Status { get; set; } = TicketStatus.Pending;

    }
}
