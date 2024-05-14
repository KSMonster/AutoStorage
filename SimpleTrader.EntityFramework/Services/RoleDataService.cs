

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
    public class RoleDataService : IRoleService<Role>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Role> _nonQueryDataService;

        public RoleDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Role>(contextFactory);
        }

        public async Task<Role> Create(Role entity)
        {
            return await _nonQueryDataService.Create(entity);
        }
        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Role entity = await context.Set<Role>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<Role>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Role> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Role entity = await context.Role.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }
        public Role GetRole(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Role entity = context.Role.FirstOrDefault((e) => e.Id == id);
                return entity;
            }
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Role> entities = await context.Set<Role>().ToListAsync();
                return entities;
            }
        }


        public async Task<Role> Update(int id, Role entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
}
