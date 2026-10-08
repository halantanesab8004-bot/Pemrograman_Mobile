namespace BlazingPizzaApp.API.Models
{
    // Minimal model to represent a topping selection on a pizza
    public class PizzaTopping
    {
        public int Id { get; set; }

        // Reference to a Topping definition
        public int? ToppingId { get; set; }
        public Topping? Topping { get; set; }
    }
}
