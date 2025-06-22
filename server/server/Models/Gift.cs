using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public string GiftName { get; set; }
        public string? Details { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Range(10,100,ErrorMessage ="The price must between 10 and 100")]
        public int Price { get; set; }
        public string? ImageUrl { get; set; }

        public int DonorId { get; set; }
        public Donor Donor { get; set; }

        public int? WinnerId { get; set; }
        public User? Winner { get; set; }

        public List<Ticket> Tickets { get; set; }

    }
}
