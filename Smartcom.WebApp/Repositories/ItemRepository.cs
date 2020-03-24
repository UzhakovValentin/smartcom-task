using Microsoft.EntityFrameworkCore;
using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private readonly AppDataBaseContext dbContext;
    
        public ItemRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(Item entity) =>
            await dbContext.Items.AddAsync(entity);
        public async Task Delete(Guid identifier)
        {
            var item = await dbContext.Items.FindAsync(identifier);
            if (item != null)
            {
                dbContext.Items.Remove(item);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        public async Task<Item> Get(Guid identifier) =>
            await dbContext.Items.FindAsync(identifier);
        public async Task<List<Item>> GetAll() =>
            await dbContext.Items.ToListAsync();
        public void Update(Item entity) =>
            dbContext.Items.Update(entity);
        //dbContext.Entry(await dbContext.Items.FindAsync(entity.ItemId)).CurrentValues.SetValues(entity);
    }
}
