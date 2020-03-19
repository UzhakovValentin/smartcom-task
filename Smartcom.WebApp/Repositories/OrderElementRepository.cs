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
    public class OrderElementRepository : IRepository<OrderElement>
    {
        private readonly AppDataBaseContext dbContext;

        public OrderElementRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(OrderElement entity) =>
            await dbContext.OrderElements.AddAsync(entity);
        public async Task Delete(Guid identifier)
        {
            var orderElement = await dbContext.OrderElements.FindAsync(identifier);
            if (orderElement != null)
            {
                dbContext.OrderElements.Remove(orderElement);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
        public async Task<OrderElement> Get(Guid identifier) =>
            await dbContext.OrderElements.FindAsync(identifier);
        public async Task<List<OrderElement>> GetAll() =>
            await dbContext.OrderElements.ToListAsync();
        public void Update(OrderElement entity) =>
            dbContext.OrderElements.Update(entity);
    }
}
