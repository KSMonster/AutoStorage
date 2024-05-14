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
    public class ExtraItemDataService : IItemDataService<Item>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Item> _nonQueryDataService;

        public ExtraItemDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Item>(contextFactory);
        }

        public async Task<Item> Create(Item entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Item entity = await context.Set<Item>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<Item>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Item> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Item entity = await context.Items.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Item>> GetWhere(string name)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Where(i => i.Name.Equals(name)).ToListAsync();
                return entities;
            }
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Include(i=>i.Box).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Item>> GetSuggestions(string name)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Where(i => i.Name.StartsWith(name)).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Item>> GetEquals(string name)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Where(i => i.Name.Equals(name)).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<Item>> CountAvailable(string name)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Where(i => i.Name.StartsWith(name)).ToListAsync();
                return entities;
            }
        }

        public async Task<Item> Update(int id, Item entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
    public class ItemDataService : IDataService<Item>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Item> _nonQueryDataService;

        public ItemDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Item>(contextFactory);
        }

        public async Task<Item> Create(Item entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Item entity = await context.Set<Item>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<Item>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Item> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Item entity = await context.Items.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<Item>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Item> entities = await context.Set<Item>().Include(i => i.Box).ToListAsync();
                return entities;
            }
        }

        public async Task<Item> Update(int id, Item entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
}
