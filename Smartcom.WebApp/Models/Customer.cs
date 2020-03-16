using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Smartcom.WebApp.Models
{
    public class Customer : IdentityUser<Guid>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public string Address { get; set; }
        public float Discount { get; set; }

        public List<Order> Orders { get; set; }
    }
}
