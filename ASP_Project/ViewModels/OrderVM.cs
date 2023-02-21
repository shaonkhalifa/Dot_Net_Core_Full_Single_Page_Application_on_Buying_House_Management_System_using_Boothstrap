using ASP_Project.Models;

namespace ASP_Project.ViewModels
{
    public class OrderVM
    {

        public OrderVM()
        {
            this.OrderDetails = new List<OrderDetailVM>();
        }
        public Order Order { get; set; } = null!;

        public Buyer Buyer { get; set; } = null!;
        public List<OrderDetailVM> OrderDetails { get; set; } = null!;
    }
}
