using BlazingPizzaApp.API.Models;

namespace BlazingPizzaApp.API.Data
{
    public interface IPizzaSpecials
    {
        Task<IEnumerable<PizzaSpecial>> GetPizzaSpecialsAsync();
    }
}
