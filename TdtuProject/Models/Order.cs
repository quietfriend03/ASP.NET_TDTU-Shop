namespace TdtuProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice()
        {
            return Product.ProductPrice * Quantity;
        }
    }
}
