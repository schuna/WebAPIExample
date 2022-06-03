using Microsoft.EntityFrameworkCore;

namespace WebApi.Models
{
    public class ShopContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShopContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(a => a.Category)
                .HasForeignKey(a => a.CategoryId);
            modelBuilder.Entity<Order>().HasMany(o => o.Products);
            modelBuilder.Entity<Order>().HasOne(o => o.User);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);
            modelBuilder.Seed();
        }
    }
}