using System.ComponentModel.DataAnnotations;

namespace Smartcom.WebApp.ViewModels.Requests
{
    public class AddItemRequest
    {
        public string Name { get; set; }
        public float Price { get; set; }
        [MaxLength(30)]
        public string Category { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
