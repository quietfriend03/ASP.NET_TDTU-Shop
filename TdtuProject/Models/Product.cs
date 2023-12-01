using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TdtuProject.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string FileName { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

}
