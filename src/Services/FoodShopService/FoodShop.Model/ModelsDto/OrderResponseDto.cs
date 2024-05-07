namespace FoodShop.Model.ModelsDto
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<OrderDetailResponseDto> OrderDetails { get; set; }
    }
}
