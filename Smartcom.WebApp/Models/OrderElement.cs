using System;
using System.ComponentModel.DataAnnotations;

namespace Smartcom.WebApp.Models
{
    public class OrderElement
    {
        public Guid OrderElementId { get; set; }
        [Required]
        public int ItemsCount { get; set; }
        [Required]
        public float ItemPrice { get; set; }

        [Required]
        public Guid ItemId { get; set; }
        public Item Item { get; set; }
        [Required]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
    }
}
