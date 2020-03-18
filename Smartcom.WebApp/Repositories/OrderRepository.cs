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
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDataBaseContext dbContext;

        public OrderRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(Order entity) =>
            await dbContext.Orders.AddAsync(entity);

        public async Task Delete(Guid identifier)
        {
            var order = await dbContext.Orders.FindAsync(identifier);
            if (order != null)
            {
                dbContext.Orders.Remove(order);
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public async Task<Order> Get(Guid identifier) =>
            await dbContext.Orders.FindAsync(identifier);

        public async Task<List<Order>> GetAll() =>
            await dbContext
                    .Orders
                    .Include(o => o.OrderElemnts)
                    .ThenInclude(o => o.Item)
                    .ToListAsync();

        public void Update(Order entity) =>
            dbContext.Entry(entity).State = EntityState.Modified;
    }
}
