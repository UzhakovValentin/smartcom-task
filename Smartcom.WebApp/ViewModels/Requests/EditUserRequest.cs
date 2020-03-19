using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.ViewModels.Requests
{
    public class EditUserRequest
    {
        public Guid EditedUserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Discount { get; set; }
    }
}
