namespace FoodShop.Model.ModelsDto
{
    public class OrderDetailResponseDto
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public decimal FoodPrice { get; set; }
        public int Quantity { get; set; }
    }
}
