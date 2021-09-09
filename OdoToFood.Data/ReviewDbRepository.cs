using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Data
{
    public class ReviewDbRepository : IReviewRepository
    {
        private OdeToFoodContext _context;
        public ReviewDbRepository(OdeToFoodContext odeToFoodContext)
        {
            _context = odeToFoodContext;
        }
        public async Task<Review> AddAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task DeleteAsync(int id)
        {
            var review = await GetByIdAsync(id);
            _context.Reviews.Remove(review);
             await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IReadOnlyList<Review>> GetReviewsByRestaurantAsync(int id)
        {
            return await _context.Reviews.Where(x => x.RestaurantId == id).ToListAsync();
        }

        public async Task UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
    }
}
