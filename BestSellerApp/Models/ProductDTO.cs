
using System.ComponentModel.DataAnnotations;

namespace BestSellerApp.Models
{
    public class ProductDTO
    {
        [Required,MaxLength(1000)]
        public string Name { get; set; }   
        [Required ,MaxLength(100)]
        public string? Brand { get; set; }
        [Required ,MaxLength(100)]
        public string? Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string Desciption { get; set; } = "";     
        public IFormFile ? Image { get; set; }
    }
}
