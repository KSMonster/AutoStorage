using Microsoft.EntityFrameworkCore;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.EntityFramework.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SimpleTrader.EntityFramework.Services
{
    public class LogDataService : IDataService<Log>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Log> _nonQueryDataService;

        public LogDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Log>(contextFactory);
        }

        public async Task<Log> Create(Log entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Log entity = await context.Set<Log>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<Log>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Log> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Log entity = await context.Logs.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Log>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Log> entities = await context.Set<Log>().ToListAsync();
                return entities;
            }
        }

        public async Task<Log> Update(int id, Log entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
}
