using Dapper;
using Microsoft.EntityFrameworkCore;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.DTOs;
using WebAppCarClub.Models;
using WebAppCarClub.ViewModels;

namespace WebAppCarClub.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DapperContext _dapperContext;

        public UserRepository(ApplicationDbContext context, DapperContext dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext;
        }
        public bool Add(User user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Dapper
        public async Task<UserClubsAndRacesDTO> DapperGetUserClubsAndRaces(string id)
        {
            var query = "SELECT * FROM Clubs WHERE UserId = @Id;" + 
                        "SELECT * FROM Races WHERE UserId = @Id;";

            using (var connection = _dapperContext.CreateConnection())
            using (var multi = await connection.QueryMultipleAsync(query, new { id }))
            {
                var userClubs = await multi.ReadAsync<Club>();
                var userRaces = await multi.ReadAsync<Race>();

                var dto = new UserClubsAndRacesDTO
                {
                    UserClubs = userClubs.ToList(),
                    UserRaces = userRaces.ToList()
                };

                return dto;
            }
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }
    }
}
