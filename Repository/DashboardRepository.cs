using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Models;

namespace WebAppCarClub.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DapperContext _dapperContext;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, DapperContext dapperContext)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _dapperContext = dapperContext;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.User.Id == currentUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.User.Id == currentUser);
            return userRaces.ToList();
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        
        public async Task<User> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }

        // Dapper
        public async Task<User> DapperGetUserProfileImage(string id)
        {
            var query = "SELECT ProfileImageUrl FROM AspNetUsers WHERE Id = @Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                var userProfileImage = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });
                return userProfileImage;
            }
        }
        public async Task DapperUpdateUser(User user)
        {
            var query = "UPDATE AspNetUsers SET UserName = @UserName, Speed = @Speed, Mileage = @Mileage, ProfileImageUrl = @ProfileImageUrl, City = @City, State = @State, Bio = @Bio WHERE Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", user.Id, DbType.String);
            parameters.Add("UserName", user.UserName, DbType.String);
            parameters.Add("Speed", user.Speed, DbType.Int32);
            parameters.Add("Mileage", user.Mileage, DbType.Int32);
            parameters.Add("ProfileImageUrl", user.ProfileImageUrl, DbType.String);
            parameters.Add("City", user.City, DbType.String);
            parameters.Add("State", user.State, DbType.String);
            parameters.Add("Bio", user.Bio, DbType.String);
            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task DapperDeleteUser(string id)
        {
            var query = "DELETE FROM AspNetUsers WHERE Id = @Id";
            using (var connection = _dapperContext.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id });
            }
        }

        public bool Update(User user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
