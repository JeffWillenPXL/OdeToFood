using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace OdeToFood.Data
{
    public class RestaurantDbRepository : IRestaurantRepository
    {
        private readonly OdeToFoodContext _context;
        public RestaurantDbRepository(OdeToFoodContext odeToFoodContext)
        {
            _context = odeToFoodContext;
        }
        public void Delete(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<Restaurant> GetAll()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Restaurant Create(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public Restaurant Update(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }
    }
}
