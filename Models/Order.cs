using System.Collections.Generic;

namespace BlazingPizzaApp.API.Models
{
    // Simple Order model following course structure
    public class Order
    {
        public Order()
        {
            Pizzas = new List<Pizza>();
        }

        // Order identifier
        public int OrderId { get; set; }

        // Delivery address (optional for now)
        public string? Address { get; set; }

        // Collection of pizzas associated with this order
        public List<Pizza> Pizzas { get; }
    }
}
