using WebAppCarClub.Models;

namespace WebAppCarClub.ViewModels
{
    public class UserDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Speed { get; set; }
        public int? Mileage { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Bio { get; set; }
        public List<Club> Clubs { get; set; }
        public List<Race> Races { get; set; }
    }
}
