using System;
using BlazingPizzaApp.API.Models;

namespace BlazingPizzaApp.API.Services
{
    // In-memory order state service
    public class OrderState
    {
        public OrderState()
        {
            CurrentOrder = new Order();
        }

        // The active order being built
        public Order CurrentOrder { get; private set; }

        // Event to notify listeners when the order changes (useful for UI updates)
        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();

        // Add a pizza to the current order
        public void AddPizza(Pizza pizza)
        {
            if (pizza == null) return;
            CurrentOrder.Pizzas.Add(pizza);
            NotifyStateChanged();
        }

        // Remove a pizza from the current order
        public void RemovePizza(Pizza pizza)
        {
            if (pizza == null) return;
            CurrentOrder.Pizzas.Remove(pizza);
            NotifyStateChanged();
        }

        // Clear/reset the current order
        public void ClearOrder()
        {
            CurrentOrder = new Order();
            NotifyStateChanged();
        }
    }
}
