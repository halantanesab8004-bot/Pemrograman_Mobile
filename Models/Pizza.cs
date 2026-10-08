using System.Collections.Generic;

namespace BlazingPizzaApp.API.Models
{
    // Represents a pizza in an order (not to be confused with PizzaSpecial)
    public class Pizza
    {
        public Pizza()
        {
            PizzaToppings = new List<PizzaTopping>();
        }

        // Unique id for this pizza (order item)
        public int Id { get; set; }

        // Reference to parent order
        public int OrderId { get; set; }

        // Default size (e.g., Small, Medium, Large)
        public string DefaultSize { get; set; } = "Medium";

        // Link to the PizzaSpecial selected
        public int? PizzaSpecialId { get; set; }
        public PizzaSpecial? PizzaSpecial { get; set; }

        // Collection of toppings chosen for this pizza
        public List<PizzaTopping> PizzaToppings { get; set; }
    }
}
