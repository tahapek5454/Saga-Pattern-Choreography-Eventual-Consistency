namespace Order.API.ViewModels
{
    public class OrderItemVM
    {
        public string ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
