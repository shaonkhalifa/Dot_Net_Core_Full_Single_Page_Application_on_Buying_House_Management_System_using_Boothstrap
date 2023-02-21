using System.ComponentModel.DataAnnotations;

namespace ASP_Project.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Price { get; set; }
   
        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturingDate { get; set; }
        public string PicturePath { get; set; } = default!;
        public bool InStock { get; set; }
        public IFormFile Picture { get; set; } = default!;
    }
}
