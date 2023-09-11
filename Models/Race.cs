using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebAppCarClub.Data.Enum;

namespace WebAppCarClub.Models
{
    public class Race
    {
        [Key] 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public DateTime? StartTime { get; set; }
        public int? EntryFee { get; set; }
        public string? Website { get; set; }
        public string? Twitter { get; set; }
        public string? Facebook { get; set; }
        public string? Contact { get; set; }
        public RaceCategory RaceCategory { get; set; }

        [ForeignKey("Address")] 
        public int AddressId { get; set; }
        public Address Address { get; set; }

        [ForeignKey("User")] 
        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
