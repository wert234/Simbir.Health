using Account.Domain.Entitys.Account;
using Account.Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Sherad.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Infrastructure.Repositories
{
    public class AccountRepository : IRepository<User, int>
    {
        private readonly AccountDbContext _context;


        public AccountRepository(AccountDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(User entity)
        {
           await _context.AddAsync(entity);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            _context.Users.Remove(await GetAsync(id));
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await _context.SaveChangesAsync(token);
        }

        public async Task UpdateAsync(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
