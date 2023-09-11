using WebAppCarClub.Data.Enum;
using WebAppCarClub.Models;

namespace WebAppCarClub.ViewModels
{
    public class CreateRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public RaceCategory RaceCategory { get; set; }
        public string UserId { get; set; } // Identity to know who's created the data
    }
}
