using Microsoft.EntityFrameworkCore;
using Loop.Models.Common;

namespace Loop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BusinessMember> BusinessMembers { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Payout> Payouts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Business>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<BusinessMember>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Cart>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<CartItem>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Category>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Customer>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Favorite>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Inventory>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Order>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<OrderItem>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Payout>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ProductImage>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ProductTag>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Review>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<Tag>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");

            // Foreign Keys
            modelBuilder.Entity<Address>()
                .HasMany<Order>(a => a.Orders)
                .WithOne(o => o.Address)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Business>()
                .HasMany<BusinessMember>(b => b.BusinessMembers)
                .WithOne(bm => bm.Business)
                .HasForeignKey(bm => bm.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Business>()
                .HasMany<OrderItem>(b => b.OrderItems)
                .WithOne(oi => oi.Business)
                .HasForeignKey(oi => oi.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Business>()
                .HasMany<Payout>(b => b.Payouts)
                .WithOne(p => p.Business)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
                
            modelBuilder.Entity<Business>()
                .HasMany<Product>(b => b.Products)
                .WithOne(p => p.Business)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasMany<CartItem>(c => c.CartItems)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasMany<Product>(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Address>(c => c.Addresses)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Favorite>(c => c.Favorites)
                .WithOne(f => f.Customer)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Order>(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany<Review>(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany<OrderItem>(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasMany<Payment>(o => o.Payments)
                .WithOne(p => p.Order)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasMany<CartItem>(p => p.CartItems)
                .WithOne(ci => ci.Product)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<Favorite>(p => p.Favorites)
                .WithOne(f => f.Product)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<Inventory>(p => p.Inventories)
                .WithOne(i => i.Product)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<OrderItem>(p => p.OrderItems)
                .WithOne(oi => oi.Product)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<ProductImage>(p => p.ProductImages)
                .WithOne(pi => pi.Product)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<ProductTag>(p => p.ProductTags)
                .WithOne(pt => pt.Product)
                .HasForeignKey(pt => pt.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Product>()
                .HasMany<Review>(p => p.Reviews)
                .WithOne(r => r.Product)
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasMany<ProductTag>(t => t.ProductTags)
                .WithOne(pt => pt.Tag)
                .HasForeignKey(pt => pt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasOne<Customer>(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany<BusinessMember>(u => u.BusinessMembers)
                .WithOne(bm => bm.User)
                .HasForeignKey(bm => bm.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
