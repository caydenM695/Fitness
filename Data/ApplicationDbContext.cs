﻿using Fitness.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fitness.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<MealPlan> MealPlans { get; set; }
        public DbSet<UserMeal> UserIntakes { get; set; }
        public DbSet<UserWeight> UserWeights { get; set; }

    }
}
