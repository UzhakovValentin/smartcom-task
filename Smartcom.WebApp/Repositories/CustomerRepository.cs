using Smartcom.WebApp.Database;
using Smartcom.WebApp.Models;
using Smartcom.WebApp.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly AppDataBaseContext dbContext;

        public CustomerRepository(AppDataBaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Create(Customer entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public Customer Get(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Customer entity)
        {
            throw new NotImplementedException();
        }
    }
}
