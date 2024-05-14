using Microsoft.EntityFrameworkCore;
using SimpleTrader.Domain.Models;
using SimpleTrader.Domain.Services;
using SimpleTrader.EntityFramework.Services.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SimpleTrader.EntityFramework.Services
{
    public class BoxDataService : IDataService<Box>
    {
        private readonly SimpleTraderDbContextFactory _contextFactory;
        private readonly NonQueryDataService<Box> _nonQueryDataService;

        public BoxDataService(SimpleTraderDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
            _nonQueryDataService = new NonQueryDataService<Box>(contextFactory);
        }

        public async Task<Box> Create(Box entity)
        {
            return await _nonQueryDataService.Create(entity);
        }
        public async Task<Box> CreateNew(int count)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                int lastBoxNumber = await context.Boxes.OrderByDescending(b => b.BoxNumber).Select(b => b.BoxNumber).FirstOrDefaultAsync();
                List<Box> newBox = new List<Box>();
                for (int i = 0; i < count; i++)
                {
                    Box box = new Box { BoxNumber = lastBoxNumber + i + 1};
                    newBox.Add(box);
                }
                context.Boxes.AddRange(newBox);
                await context.SaveChangesAsync();

                return null;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Box entity = await context.Set<Box>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<Box>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Box> Get(int id)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Box entity = await context.Boxes.FirstOrDefaultAsync((e) => e.Id == id);
                return entity;
            }
        }
        public async Task<Box> GetNewest()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                Box entity = await context.Boxes.LastOrDefaultAsync();
                return entity;
            }
        }

        public async Task<IEnumerable<Box>> GetAll()
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<Box> entities = await context.Set<Box>().ToListAsync();
                return entities;
            }
        }

        public async Task<Box> Update(int id, Box entity)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await _nonQueryDataService.Update(id, entity);
            }
        }
        public async Task<IEnumerable<Box>> GetBoxesForItems(IEnumerable<Item> items)
        {
            using (SimpleTraderDbContext context = _contextFactory.CreateDbContext())
            {
                var boxIds = items.Select(item => item.fk_BoxId).Distinct();

                var boxes = await context.Boxes.Where(box => boxIds.Contains(box.Id)).ToListAsync();

                return boxes;
            }
        }
    }
}
