using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;

namespace ShelfLife.Api.Data;

public class ShelfLifeDbContext : DbContext
{
    public ShelfLifeDbContext(DbContextOptions<ShelfLifeDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<FoodProduct> FoodProducts { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Household> Households { get; set; }
    public DbSet<UserHousehold> UserHouseholds { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // Category entity configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Color).HasMaxLength(7); // For hex color codes
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // FoodProduct entity configuration
        modelBuilder.Entity<FoodProduct>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.HasIndex(e => e.Barcode);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationship
            entity.HasOne(e => e.Category)
                .WithMany(c => c.FoodProducts)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // InventoryItem entity configuration
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(10, 3);
            entity.Property(e => e.Unit).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.InventoryItems)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FoodProduct)
                .WithMany(fp => fp.InventoryItems)
                .HasForeignKey(e => e.FoodProductId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Household)
                .WithMany(h => h.InventoryItems)
                .HasForeignKey(e => e.HouseholdId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Household entity configuration
        modelBuilder.Entity<Household>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // UserHousehold entity configuration (many-to-many junction table)
        modelBuilder.Entity<UserHousehold>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.HouseholdId });
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.JoinedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserHouseholds)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Household)
                .WithMany(h => h.UserHouseholds)
                .HasForeignKey(e => e.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ShoppingList entity configuration
        modelBuilder.Entity<ShoppingList>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationships
            entity.HasOne(e => e.User)
                .WithMany(u => u.ShoppingLists)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Household)
                .WithMany(h => h.ShoppingLists)
                .HasForeignKey(e => e.HouseholdId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // ShoppingListItem entity configuration
        modelBuilder.Entity<ShoppingListItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomItemName).HasMaxLength(200);
            entity.Property(e => e.Quantity).HasPrecision(10, 3);
            entity.Property(e => e.Unit).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EstimatedPrice).HasPrecision(10, 2);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationships
            entity.HasOne(e => e.ShoppingList)
                .WithMany(sl => sl.ShoppingListItems)
                .HasForeignKey(e => e.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FoodProduct)
                .WithMany(fp => fp.ShoppingListItems)
                .HasForeignKey(e => e.FoodProductId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Recipe entity configuration
        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Instructions).IsRequired().HasMaxLength(5000);
            entity.Property(e => e.PrepTimeMinutes).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationship
            entity.HasOne(e => e.Household)
                .WithMany(h => h.Recipes)
                .HasForeignKey(e => e.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // RecipeIngredient entity configuration
        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(10, 3);
            entity.Property(e => e.UnitOfMeasure).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key relationships
            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.FoodProduct)
                .WithMany(fp => fp.RecipeIngredients)
                .HasForeignKey(e => e.FoodProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique constraint to prevent duplicate ingredients in the same recipe
            entity.HasIndex(e => new { e.RecipeId, e.FoodProductId }).IsUnique();
        });
    }
}