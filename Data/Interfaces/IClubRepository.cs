using WebAppCarClub.Models;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Data.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Club>> GetAllClubsByCity(string city);

        Task<IEnumerable<Club>> DapperGetClubs();
        Task<Club> DapperGetClubById(int id);
        Task<IEnumerable<Club>> DapperGetClubsByCity(string city);
        Task<Club> DapperGetClubLatest();
        Task<Club> DapperCreateClub(Club club);

        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
