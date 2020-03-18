using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.ViewModels.Requests
{
    public class EditItemRequest
    {
        public Guid ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string Category { get; set; }
    }
}
