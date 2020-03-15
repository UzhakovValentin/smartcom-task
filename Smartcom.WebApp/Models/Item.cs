using System;
using System.ComponentModel.DataAnnotations;

namespace Smartcom.WebApp.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        [Required]
        public string Code { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        [MaxLength(30)]
        public string Category { get; set; }
    }
}
