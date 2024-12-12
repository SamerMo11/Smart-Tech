using E_CommerceWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWeb.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<state> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DiscountCode> DiscountCodes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<CategoryAttribute> categoryAttributes { get; set; }
        public DbSet<ProductVariantImage> VariantImages { get; set; }
        public DbSet<ProductVariant> Variants { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<CityDelivery> cityDeliveries { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferProduct> OfferProducts { get; set; }
        public DbSet<Search> Searches { get; set; }



     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Favourite>().HasKey(x => new { x.User_Id, x.ProductIn_Id });

            builder.Entity<Category>().HasQueryFilter(x => !x.IsDeleted.Value);
            builder.Entity<Product>().HasQueryFilter(x => !x.IsDeleted.Value);
            builder.Entity<ProductVariant>().HasQueryFilter(x => !x.IsDeleted.Value);
            builder.Entity<ApplicationUser>().HasQueryFilter(x => !x.IsDeleted);


            //builder.Entity<IdentityRole>().HasData(
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "user",
            //    NormalizedName = "USER",
            //    ConcurrencyStamp = Guid.NewGuid().ToString()
            //},
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "admin",
            //    NormalizedName = "ADMIN",
            //    ConcurrencyStamp = Guid.NewGuid().ToString()
            //},
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "manager",
            //    NormalizedName = "MANAGER",
            //    ConcurrencyStamp = Guid.NewGuid().ToString()
            //},
            //new IdentityRole
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Name = "delivery",
            //    NormalizedName = "DELIVERY",
            //    ConcurrencyStamp = Guid.NewGuid().ToString()
            //});

            base.OnModelCreating(builder);
        }

    }
}
