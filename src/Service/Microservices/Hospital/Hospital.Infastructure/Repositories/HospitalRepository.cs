using Hospital.Infastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Infastructure.Repositories
{
    public class HospitalRepository : IRepository<Hospital.Domain.Entitys.Hospital, Guid>
    {

        private readonly HospitalDbContext _hospitalDbContext;


        public HospitalRepository(HospitalDbContext hospitalDbContext)
        {
            _hospitalDbContext = hospitalDbContext;
        }


        public async Task AddAsync(Domain.Entitys.Hospital entity)
        {
            await _hospitalDbContext.AddAsync(entity);
            await _hospitalDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
             _hospitalDbContext.Remove(await GetAsync(id));
            await _hospitalDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entitys.Hospital>> GetAllAsync()
        {
            return await _hospitalDbContext.Hospitals.ToArrayAsync();
        }

        public async Task<Domain.Entitys.Hospital> GetAsync(Guid id)
        {
            return await _hospitalDbContext.Hospitals.FirstOrDefaultAsync(hospital => hospital.Id == id);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _hospitalDbContext.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(Domain.Entitys.Hospital entity)
        {
            _hospitalDbContext.Hospitals.Update(entity);
            await _hospitalDbContext.SaveChangesAsync();
        }
    }
}
