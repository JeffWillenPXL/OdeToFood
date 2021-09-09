using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;

namespace OdeToFood.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var restaurants = _restaurantRepository.GetAll();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if(restaurant != null)
            {
                return Ok(restaurant);
                
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Post([FromBody] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                var model = _restaurantRepository.Create(restaurant);
                return CreatedAtAction(nameof(Get), new { model.Id }, model);
            }
            return BadRequest();


        }


        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Restaurant restaurant, int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (_restaurantRepository.GetById(id) == null)
            {
                return NotFound();
            }
            if (restaurant.Id != id)
            {
                return BadRequest();
            }
            

            _restaurantRepository.Update(restaurant);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            _restaurantRepository.Delete(restaurant);
            return Ok();
        }
    }
}
