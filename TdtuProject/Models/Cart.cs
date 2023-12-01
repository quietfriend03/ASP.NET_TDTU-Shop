namespace TdtuProject.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal TotalPrice => Orders.Sum(order => order.TotalPrice());
    }

}
