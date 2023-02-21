using ASP_Project.Models;

namespace ASP_Project.ViewModels
{
    public class DataVM
    {
        public IEnumerable<Category> Categories { get; set; } = default!;
        public IEnumerable<Product> Products { get; set; } = default!;
        public IEnumerable<Buyer> Buyers { get; set; } = default!;
        public IEnumerable<Order> Orders { get; set; } = default!;
    }
}
