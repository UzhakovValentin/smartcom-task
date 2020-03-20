using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Smartcom.WebApp.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public DateTime ShipmentDate { get; set; }
        public float OrderNumber { get; set; }
        public string Status { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderElement> OrderElements { get; set; }
    }
}
