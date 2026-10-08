using BlazingPizzaApp.API.Data;
using BlazingPizzaApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazingPizzaApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaSpecialsController : ControllerBase
    {
        private readonly IPizzaSpecials _pizzaSpecials;

        public PizzaSpecialsController(IPizzaSpecials pizzaSpecials)
        {
            _pizzaSpecials = pizzaSpecials;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaSpecial>>> Get()
        {
            var specials = await _pizzaSpecials.GetPizzaSpecialsAsync();
            return Ok(specials);
        }
    }
}
