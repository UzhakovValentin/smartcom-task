using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.ViewModels.Requests
{
    public class ConfirmOrderRequest
    {
        public Guid OrderId { get; set; }
        public string ShipmentDate { get; set; }
    }
}
