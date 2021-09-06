using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public interface IRestaurantRepository
    {
        IReadOnlyList<Restaurant> GetAll();
        Restaurant GetById(int id);
        Restaurant Create(Restaurant restaurant);
        Restaurant Update(Restaurant restaurant);
        void Delete(Restaurant restaurant);

    }
}
