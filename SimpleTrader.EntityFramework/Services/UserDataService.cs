

using Microsoft.EntityFrameworkCore;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.EntityFramework.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace SimpleTrader.EntityFramework.Services
{
    public class UserDataService : IDataService<User>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<User> _nonQueryDataService;

        public UserDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<User>(contextFactory);
        }

        public async Task<User> Create(User entity)
        {
            return await _nonQueryDataService.Create(entity);
        }
        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                User entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<User>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<User> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                User entity = await context.Users.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<User> entities = await context.Set<User>().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<User>> GetAllWhere(string username)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<User> entities = await context.Set<User>().Where(i => i.Username.Equals(username)).ToListAsync();
                return entities;
            }
        }

        public async Task<User> Update(int id, User entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
}
