using Microsoft.EntityFrameworkCore;
using WebAppCarClub.Data;
using WebAppCarClub.Models;
using WebAppCarRace.Data.Interfaces;

namespace WebAppCarClub.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext context;

        public RaceRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool Add(Race race)
        {
            this.context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            this.context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await this.context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await this.context.Races.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
        }
        
        public async Task<Race> GetByIdAsyncNoTracking(int id)
        {
            return await this.context.Races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Race>> GetAllRacesByCity(string city)
        {
            return await this.context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = this.context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            this.context.Update(race);
            return Save();
        }
    }
}
