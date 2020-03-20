using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.ViewModels.Requests
{
    public class AddItemToCartRequest
    {
        public Guid ItemId { get; set; }
        public int ItemsCount { get; set; }


    }
}
