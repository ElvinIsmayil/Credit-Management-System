using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            // Primary key from BaseEntity (Id)
            builder.HasKey(p => p.Id);

            // Property configurations
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.StockQuantity)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(255);

            // Relationships

            // Product has one Category, with many Products
            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product has one Branch, with many Products
            builder.HasOne(p => p.Branch)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index on Name for faster searches
            builder.HasIndex(p => p.Name);
        }
    }
}
