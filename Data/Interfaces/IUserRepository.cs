using WebAppCarClub.DTOs;
using WebAppCarClub.Models;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(string id);
        Task<UserClubsAndRacesDTO> DapperGetUserClubsAndRaces(string id);
        bool Add(User user);
        bool Update(User user);
        bool Delete(User user);
        bool Save();
    }
}
