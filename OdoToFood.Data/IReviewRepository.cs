using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data
{
    public interface IReviewRepository
    {
        Task<IReadOnlyList<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(int id);
        Task<IReadOnlyList<Review>> GetReviewsByRestaurantAsync(int id);
        Task<Review> AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);


    }
}
