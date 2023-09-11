using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebAppCarClub.Data;
using WebAppCarClub.Data.Interfaces;
using WebAppCarClub.Models;

namespace WebAppCarClub.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext context;

        public ClubRepository(ApplicationDbContext context)
        {
            this.context = context;
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
