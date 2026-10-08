using BlazingPizzaApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazingPizzaApp.API.Data
{
    public class PizzaSpecialsDAL : IPizzaSpecials
    {
        private readonly PizzaStoreContext _context;
        public PizzaSpecialsDAL(PizzaStoreContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PizzaSpecial>> GetPizzaSpecialsAsync()
        {
            return await _context.PizzaSpecials.ToListAsync();
        }
    }
}
