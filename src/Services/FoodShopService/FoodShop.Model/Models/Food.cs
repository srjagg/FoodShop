﻿namespace FoodShop.Model.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
