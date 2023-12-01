namespace TdtuProject.Models
{
    public class CheckoutViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
