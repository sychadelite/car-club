using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAppCarClub.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext context;
        private readonly DapperContext dapperContext;

        public ClubRepository(ApplicationDbContext context, DapperContext dapperContext)
        {
            this.context = context;
            this.dapperContext = dapperContext;
        }
        public bool Add(Club club)
        {
            this.context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            this.context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await this.context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await this.context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await this.context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetAllClubsByCity(string city)
        {
            return await this.context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        // Dapper
        public async Task<IEnumerable<Club>> DapperGetClubs()
        {
            var query = "SELECT * FROM Clubs";
            using (var connection = this.dapperContext.CreateConnection())
            {
                var clubs = await connection.QueryAsync<Club>(query);
                return clubs.ToList();
            }
        }
        public async Task<Club> DapperGetClubById(int id)
        {
            var query = "SELECT * FROM Clubs WHERE Id = @Id";
            using (var connection = this.dapperContext.CreateConnection())
            {
                var club = await connection.QuerySingleOrDefaultAsync<Club>(query, new { id });
                return club;
            }
        }
        public async Task<IEnumerable<Club>> DapperGetClubsByCity(string city)
        {
            var query = "SELECT x.*, y.City, y.Street FROM Clubs x JOIN Addresses y ON x.AddressId = y.Id WHERE y.City = @city";
            using (var connection = this.dapperContext.CreateConnection())
            {
                var clubs = await connection.QueryAsync<Club>(query, new { city });
                return clubs.ToList();
            }
        }
        public async Task<Club> DapperGetClubLatest()
        {
            var query = "SELECT TOP(1) * FROM Clubs ORDER BY Id DESC";
            using (var connection = this.dapperContext.CreateConnection())
            {
                var club = await connection.QuerySingleOrDefaultAsync<Club>(query);
                return club;
            }
        }
        public async Task<Club> DapperCreateClub(Club club)
        {
            var query = "INSERT INTO Clubs (Title, Description, Image, ClubCategory, AddressId, UserId) VALUES (@Title, @Description, @Image, @ClubCategory, @AddressId, @UserId)" +
                        "SELECT CAST(SCOPE_IDENTITY() as int)";

            var parameters = new DynamicParameters();
            parameters.Add("Title", club.Title, DbType.String);
            parameters.Add("Description", club.Description, DbType.String);
            parameters.Add("Image", club.Image, DbType.String);
            parameters.Add("ClubCategory", club.ClubCategory, DbType.Int16);
            parameters.Add("AddressId", 1, DbType.Int16);
            parameters.Add("UserId", club.UserId, DbType.String);

            using (var connection = this.dapperContext.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdClub = new Club
                {
                    Id = id,
                    Title = club.Title,
                    Description = club.Description,
                    Image = club.Image,
                    ClubCategory = club.ClubCategory,
                    AddressId = club.AddressId,
                    UserId = club.UserId
                };

                return createdClub;
            }
        }

        public bool Save()
        {
            var saved = this.context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            this.context.Update(club);
            return Save();
        }
    }
}
