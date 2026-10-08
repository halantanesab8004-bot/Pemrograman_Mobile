using System;

namespace BlazingPizzaApp.API.Models
{
    public class OrderItem
    {
        public OrderItem()
        {
        }

        public OrderItem(PizzaSpecial pizza, string size, int quantity)
        {
            Pizza = pizza ?? throw new ArgumentNullException(nameof(pizza));
            Size = size ?? string.Empty;
            Quantity = quantity > 0 ? quantity : 1;
            UnitPrice = pizza.BasePrice;
        }

        // The pizza special being ordered
        public PizzaSpecial Pizza { get; set; } = new PizzaSpecial();

        // Size or variant (e.g., Small, Medium, Large). Kept as string for simplicity.
        public string Size { get; set; } = string.Empty;

        // Number of this pizza in the order item
        public int Quantity { get; set; } = 1;

        // Price per unit. Default initialized from Pizza.BasePrice but can be adjusted.
        public decimal UnitPrice { get; set; }

        // Computed subtotal for this order item
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
