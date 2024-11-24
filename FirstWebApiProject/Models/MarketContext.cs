using FirstWebApiProject.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApiProject.Models
{
    public class MarketContext :IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories  { get; set; }
        public virtual DbSet<ProductReview> ProductReviews  { get; set; }
        public virtual DbSet<Payment> Payments  { get; set; }
        public virtual DbSet<Order> Orders  { get; set; }
        public virtual DbSet<OrderItem> OrderItems  { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts  { get; set; }

        public MarketContext(DbContextOptions<MarketContext> options) : base(options) 
        {
            
        }

    }
}
