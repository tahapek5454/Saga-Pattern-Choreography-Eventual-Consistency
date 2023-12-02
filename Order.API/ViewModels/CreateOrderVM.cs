namespace Order.API.ViewModels
{
    public class CreateOrderVM
    {
        public string BuyerId { get; set; }
        public List<OrderItemVM> OrderItems { get; set; }
    }
}
