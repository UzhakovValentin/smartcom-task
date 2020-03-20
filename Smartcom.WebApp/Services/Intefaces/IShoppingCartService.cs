using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Services.Intefaces
{
    public interface IShoppingCartService<TOrder> where TOrder : class
    {
        void AddToList(TOrder order);
        void Remove(TOrder order);
        TOrder FindById(Guid id);
        TOrder FindByCustomerId(Guid customerId);
    }
}
