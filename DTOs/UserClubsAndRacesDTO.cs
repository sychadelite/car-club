using WebAppCarClub.Models;

namespace WebAppCarClub.DTOs
{
    public class UserClubsAndRacesDTO
    {
        public List<Club> UserClubs { get; set; }
        public List<Race> UserRaces { get; set; }
    }
}
