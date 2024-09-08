using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BestSellerApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }   //="" we intitlize it with empty string 
        [MaxLength(100)]
        public string? Brand { get; set; }
        [MaxLength(100)]
        public string? Category { get; set; }
        [Precision(16, 2)]
        public decimal Price { get; set; }

        public string Desciption { get; set; } = "";
        [MaxLength(100)]
        public string ImageFile { get; set; } = "";
        public DateTime CreatedAt { get; set; }

    }
}
