using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OdeToFood.Data;
using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OdeToFood.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private IRestaurantRepository _restaurantRepository;

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

        [HttpGet]
        public IActionResult Get(int id)
        {
            var restaurant = _restaurantRepository.GetById(id);
            if(restaurant != null)
            {
                return Ok(restaurant);
                
            }
            return NotFound();
        }
    }
}
