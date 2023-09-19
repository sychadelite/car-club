using WebAppCarClub.Models;

namespace WebAppCarClub.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Club> Clubs { get; set; }
        public Club LatestClub { get; set; }
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Location { get; set; }
        public string Org { get; set; }
        public string Postal { get; set; }
        public string Timezone { get; set; }

    }
}
