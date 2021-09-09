using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OdeToFood.Data.DomainClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace OdeToFood.Data
{
    public class OdeToFoodContext : IdentityDbContext<User, Role, int>
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public OdeToFoodContext(DbContextOptions options) : base(options)
        {

        }

    }
}
