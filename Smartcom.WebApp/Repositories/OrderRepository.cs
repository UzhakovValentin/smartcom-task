using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories
{
    public class OrderRepository : IRepository<OrderElement>
    {
        private readonly AppDataBaseContext dbContext;

        public OrderRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(OrderElement entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public OrderElement Get(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<OrderElement> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(OrderElement entity)
        {
            throw new NotImplementedException();
        }
    }
}
