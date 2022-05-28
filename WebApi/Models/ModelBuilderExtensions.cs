using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
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
                },
                new Product
                {
                    Id = 2,
                    Description = "",
                    CategoryId = 1,
                    Name = "Polo Shirt",
                    Sku = "AWMPS",
                    Price = 35,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 3,
                    Description = "",
                    CategoryId = 1,
                    Name = "Skater Graphic T-Shirt",
                    Sku = "AWMSGT",
                    Price = 33,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 4,
                    Description = "",
                    CategoryId = 1,
                    Name = "Slicker Jacket",
                    Sku = "AWMSJ",
                    Price = 125,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 5,
                    Description = "",
                    CategoryId = 1,
                    Name = "Thermal Fleece Jacket",
                    Sku = "AWMTFJ",
                    Price = 60,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 6,
                    Description = "",
                    CategoryId = 1,
                    Name = "Unisex Thermal Vest",
                    Sku = "AWMUTV",
                    Price = 95,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 7,
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Pullover",
                    Sku = "AWMVNP",
                    Price = 65,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 8,
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck Sweater",
                    Sku = "AWMVNS",
                    Price = 65,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 9,
                    Description = "",
                    CategoryId = 1,
                    Name = "V-Neck T-Shirt",
                    Sku = "AWMVNT",
                    Price = 17,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 10,
                    Description = "",
                    CategoryId = 2,
                    Name = "Bamboo Thermal Ski Coat",
                    Sku = "AWWBTSC",
                    Price = 99,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 11,
                    Description = "",
                    CategoryId = 2,
                    Name = "Cross-Back Training Tank",
                    Sku = "AWWCTT",
                    Price = 0,
                    IsAvailable = false
                },
                new Product
                {
                    Id = 12,
                    Description = "",
                    CategoryId = 2,
                    Name = "Grunge Skater Jeans",
                    Sku = "AWWGSJ",
                    Price = 68,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 13,
                    Description = "",
                    CategoryId = 2,
                    Name = "Slicker Jacket",
                    Sku = "AWWSJ",
                    Price = 125,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 14,
                    Description = "",
                    CategoryId = 2,
                    Name = "Stretchy Dance Pants",
                    Sku = "AWWSDP",
                    Price = 55,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 15,
                    Description = "",
                    CategoryId = 2,
                    Name = "Ultra-Soft Tank Top",
                    Sku = "AWWUTT",
                    Price = 22,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 16,
                    Description = "",
                    CategoryId = 2,
                    Name = "Unisex Thermal Vest",
                    Sku = "AWWUTV",
                    Price = 95,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 17,
                    Description = "",
                    CategoryId = 2,
                    Name = "V-Next T-Shirt",
                    Sku = "AWWVNT",
                    Price = 17,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 18,
                    Description = "",
                    CategoryId = 3,
                    Name = "Blueberry Mineral Water",
                    Sku = "MWB",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 19,
                    Description = "",
                    CategoryId = 3,
                    Name = "Lemon-Lime Mineral Water",
                    Sku = "MWLL",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 20,
                    Description = "",
                    CategoryId = 3,
                    Name = "Orange Mineral Water",
                    Sku = "MWO",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 21,
                    Description = "",
                    CategoryId = 3,
                    Name = "Peach Mineral Water",
                    Sku = "MWP",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 22,
                    Description = "",
                    CategoryId = 3,
                    Name = "Raspberry Mineral Water",
                    Sku = "MWR",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 23,
                    Description = "",
                    CategoryId = 3,
                    Name = "Strawberry Mineral Water",
                    Sku = "MWS",
                    Price = 2.8M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 24,
                    Description = "",
                    CategoryId = 4,
                    Name = "In the Kitchen with H+ Sport",
                    Sku = "PITK",
                    Price = 24.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 25,
                    Description = "",
                    CategoryId = 5,
                    Name = "Calcium 400 IU (150 tablets)",
                    Sku = "SC400",
                    Price = 9.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 26,
                    Description = "",
                    CategoryId = 5,
                    Name = "Flaxseed Oil 100 mg (90 capsules)",
                    Sku = "SFO100",
                    Price = 12.49M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 27,
                    Description = "",
                    CategoryId = 5,
                    Name = "Iron 65 mg (150 caplets)",
                    Sku = "SI65",
                    Price = 13.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 28,
                    Description = "",
                    CategoryId = 5,
                    Name = "Magnesium 250 mg (100 tablets)",
                    Sku = "SM250",
                    Price = 12.49M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 29,
                    Description = "",
                    CategoryId = 5,
                    Name = "Multi-Vitamin (90 capsules)",
                    Sku = "SMV",
                    Price = 9.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 30,
                    Description = "",
                    CategoryId = 5,
                    Name = "Vitamin A 10,000 IU (125 caplets)",
                    Sku = "SVA",
                    Price = 11.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 31,
                    Description = "",
                    CategoryId = 5,
                    Name = "Vitamin B-Complex (100 caplets)",
                    Sku = "SVB",
                    Price = 12.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 32,
                    Description = "",
                    CategoryId = 5,
                    Name = "Vitamin C 1000 mg (100 tablets)",
                    Sku = "SVC",
                    Price = 9.99M,
                    IsAvailable = true
                },
                new Product
                {
                    Id = 33,
                    Description = "",
                    CategoryId = 5,
                    Name = "Vitamin D3 1000 IU (100 tablets)",
                    Sku = "SVD3",
                    Price = 12.49M,
                    IsAvailable = true
                });

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "adam@example.com" },
                new User { Id = 2, Email = "barbara@example.com" });
        }
    }
}