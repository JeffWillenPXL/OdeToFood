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
            _context.Restaurants.Remove(restaurant);
            _context.SaveChanges();
        }

        public IReadOnlyList<Restaurant> GetAll()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant GetById(int id)
        {
            return _context.Restaurants.Find(id);
        }

        public Restaurant Create(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            _context.SaveChanges();
            return restaurant;
        }

        public Restaurant Update(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            _context.SaveChanges();
            return restaurant;

        }

        
    }
}
