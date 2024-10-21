using History.Infastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace History.Infastructure.Repositories
{
    public class HistoryRepository : IRepository<Domain.Entitys.History, int>
    {
        private readonly HistoryDbContext _context;


        public HistoryRepository(HistoryDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Domain.Entitys.History entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _context.Remove(await GetAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entitys.History>> GetAllAsync()
        {
            return await _context.Historys.ToArrayAsync();
        }

        public async Task<Domain.Entitys.History> GetAsync(int id)
        {
            return await _context.Historys.FirstOrDefaultAsync(hospital => hospital.Id == id);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Domain.Entitys.History entity)
        {
            _context.Historys.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
