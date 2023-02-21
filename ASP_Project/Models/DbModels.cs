using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASP_Project.Models;

namespace ASP_Project.Models
{
    public  class Category
    {

        public Category()
        {
            this.Products = new List<Product>();
        }

        public int CategoryId { get; set; }
        [Required,StringLength(60)]
        public string CategoryName { get; set; } = default!;


        public virtual ICollection<Product> Products { get; set; }
    }
    public  class Buyer
    {

        public Buyer()
        {
            this.Orders = new List<Order>();
        }

        public int BuyerId { get; set; }
        [Required, StringLength(60)]
        public string BuyerName { get; set; } = default!;
        [Required, StringLength(100)]
        public string BuyerAddress { get; set; } = default!;


        public virtual ICollection<Order> Orders { get; set; }
    }
    public  class Order
    {

        public Order()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public int OrderId { get; set; }
        [ForeignKey("Buyer")]
        public int BuyerId { get; set; }

        public virtual Buyer Buyer { get; set; } = default!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
    public  class OrderDetail
    {
        public int OrderDetailId { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Order Order { get; set; } = default!;
        public virtual Product Product { get; set; } = default!;
    }
    public  class Product
    {
       
        public Product()
        {
            this.OrderDetails = new List<OrderDetail>();
        }

        public int ProductId { get; set; }
        [Required, StringLength(60)]
        public string? ProductName { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public decimal Price { get; set; } = default!;
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ManufacturingDate { get; set; }
        public string PicturePath { get; set; } = default!;
        public bool InStock { get; set; }

        public virtual Category Category { get; set; } = default!;

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

   public class BuyingHouseDbContex : DbContext
    {
        public BuyingHouseDbContex(DbContextOptions<BuyingHouseDbContex> options):base(options) 
        {

        }
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Buyer> Buyers { get; set; } = default!;
        public DbSet<OrderDetail> orderDetails { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
    }
}
