﻿using Microsoft.EntityFrameworkCore;

namespace WebApi.DataAccess.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Active Wear - Men" },
                new Category { Id = 2, Name = "Active Wear - Women" },
                new Category { Id = 3, Name = "Mineral Water" },
                new Category { Id = 4, Name = "Publications" },
                new Category { Id = 5, Name = "Supplements" });

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Description = "",
                    CategoryId = 1,
                    Name = "Grunge Skater Jeans",
                    Sku = "AWMGSJ",
                    Price = 68,
                    IsAvailable = true
                });
        }
    }
}