using FFS.Application.Entities;
using FFS.Application.Entities.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FFS.Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Chat>()
               .HasOne(p => p.FormUser)
               .WithMany()
               .HasForeignKey(p => p.FromUserId)
               .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Chat>()
                .HasOne(p => p.ToUser)
                .WithMany()
                .HasForeignKey(p => p.FromUserId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Order>().HasOne(p => p.Customer).WithMany().HasForeignKey(p => p.CustomerId).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<Order>().HasOne(p => p.Shipper).WithMany().HasForeignKey(p => p.ShipperId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Comment>()
               .HasOne(c => c.ParentComment)
               .WithMany()
               .OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Inventory>().
                HasOne(c => c.Store).WithMany(c => c.Inventories).HasForeignKey(c => c.StoreId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Inventory>().
                HasOne(c => c.Food).WithMany(c => c.Inventories).HasForeignKey(c => c.FoodId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<OrderDetail>().
               HasOne(c => c.Order).WithMany(c => c.OrderDetails).HasForeignKey(c => c.OrderId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<OrderDetail>().
               HasOne(c => c.Food).WithMany(c => c.OrderDetails).HasForeignKey(c => c.FoodId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<React>().
               HasOne(c => c.Comment).WithMany(c => c.Reacts).HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<React>().
               HasOne(c => c.User).WithMany(c => c.Reacts).HasForeignKey(c => c.UserId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Food>().
             HasOne(c => c.Store).WithMany(c => c.Foods).HasForeignKey(c => c.StoreId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<Food>().
             HasOne(c => c.Category).WithMany(c => c.Foods).HasForeignKey(c => c.CategoryId).OnDelete(DeleteBehavior.ClientNoAction);

            //builder.Entity<Comment>().
            //HasOne(c => c.ParentComment).WithMany(c => c.ParentComments).HasForeignKey(c => c.ParentCommentId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(x => x.Id).HasMaxLength(50).IsRequired(true);
            });

            builder.Entity<ApplicationRole>(entity =>
            {
                entity.Property(x => x.Id).HasMaxLength(50).IsRequired(true);
            });

            builder.Entity<FoodCombo>().
              HasOne(c => c.Food).WithMany(c => c.FoodCombos).HasForeignKey(c => c.FoodId).OnDelete(DeleteBehavior.ClientNoAction);

            builder.Entity<FoodCombo>().
             HasOne(c => c.Store).WithMany(c => c.FoodCombos).HasForeignKey(c => c.StoreId).OnDelete(DeleteBehavior.ClientNoAction);
            builder.Entity<ReactPost>().
             HasOne(c => c.Post).WithMany(c => c.ReactPosts).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.ClientNoAction);

            //builder.Entity<FoodCombo>()
            //   .HasOne(c => c.Store)
            //   .WithMany()
            //   .OnDelete(DeleteBehavior.ClientNoAction);

            //builder.Entity<ApplicationRole>()
            //    .HasData(
            //    new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Admin", Description = "Admin", NormalizedName = "ADMIN" },
            //    new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "StoreOwner", Description = "StoreOwner", NormalizedName = "STOREOWNER" },
            //    new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "Shipper", Description = "Shipper", NormalizedName = "SHIPPER" },
            //    new ApplicationRole { Id = Guid.NewGuid().ToString(), Name = "User", Description = "User", NormalizedName = "USER" }
            //    );


        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<Combo> Combos { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Food> Foods { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Inventory> Inventories { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<React> Reacts { get; set; }
        public virtual DbSet<ReactPost> ReactPosts { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Wallet> Wallets { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<FoodCombo> FoodCombos { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public override int SaveChanges()
        {
            TrackingEntities();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            TrackingEntities();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        private void TrackingEntities()
        {
            var modified = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

            foreach (EntityEntry item in modified)
            {
                var changedOrAddedItem = item.Entity as BaseEntity<int>;
                if (changedOrAddedItem != null)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedAt = DateTime.Now;
                    }
                    changedOrAddedItem.UpdatedAt = DateTime.Now;
                    //changedOrAddedItem.IsDelete = false;
                }
            }
        }

    }
}

