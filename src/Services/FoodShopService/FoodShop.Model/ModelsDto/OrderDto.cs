namespace FoodShop.Model.ModelsDto
{
    public class OrderDto
    {
        public int UserId { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
