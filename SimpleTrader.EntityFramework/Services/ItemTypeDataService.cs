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
    public class ItemTypeDataService : IDataService<ItemType>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<ItemType> _nonQueryDataService;

        public ItemTypeDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<ItemType>(contextFactory);
        }

        public async Task<ItemType> Create(ItemType entity)
        {
            return await _nonQueryDataService.Create(entity);
        }

        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                ItemType entity = await context.Set<ItemType>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<ItemType>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<ItemType> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                ItemType entity = await context.ItemTypes.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }

        public async Task<IEnumerable<ItemType>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<ItemType> entities = await context.Set<ItemType>().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<ItemType>> GetSuggestions(string name)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<ItemType> entities = await context.Set<ItemType>().Where(i => i.Name.StartsWith(name)).ToListAsync();
                return entities;
            }
        }

        public async Task<ItemType> Update(int id, ItemType entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
    }
}
