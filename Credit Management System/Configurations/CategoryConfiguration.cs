using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Primary key from BaseEntity (Id)
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(c => c.Description)
                .HasMaxLength(1000);

            builder.Property(c => c.ImageUrl)
                .HasMaxLength(255);

            // Self-referencing relationship: ParentCategory and SubCategories
            builder.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

            // One-to-many with Products
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Index on Name
            builder.HasIndex(c => c.Name);
        }
    }
}
