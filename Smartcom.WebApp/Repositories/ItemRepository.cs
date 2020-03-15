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
        public void Create(Item entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public Item Get(Guid identifier)
        {
            throw new NotImplementedException();
        }

        public List<Item> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Item entity)
        {
            throw new NotImplementedException();
        }
    }
}
