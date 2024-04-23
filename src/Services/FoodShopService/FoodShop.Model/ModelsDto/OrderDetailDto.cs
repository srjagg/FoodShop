namespace FoodShop.Model.ModelsDto
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
