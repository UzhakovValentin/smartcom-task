using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories
{
    public class OrderElementRepository : IRepository<Order>
    {
        private readonly AppDataBaseContext dbContext;

        public OrderElementRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(Order entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public Order Get(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
