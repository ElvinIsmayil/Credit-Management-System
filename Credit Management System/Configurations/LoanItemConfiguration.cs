using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class LoanItemConfiguration : IEntityTypeConfiguration<LoanItem>
    {
        public void Configure(EntityTypeBuilder<LoanItem> builder)
        {
            // Primary key inherited from BaseEntity (Id)
            builder.HasKey(li => li.Id);

            // Properties
            builder.Property(li => li.Quantity)
                .IsRequired();

            builder.Property(li => li.UnitPrice)
                .IsRequired()
                .HasPrecision(18, 2); // Proper decimal precision for money

            // TotalPrice is computed, so ignore it in EF Core mapping
            builder.Ignore(li => li.TotalPrice);

            // Relationships

            // LoanItem -> Product (many-to-one)
            builder.HasOne(li => li.Product)
                .WithMany() // Assuming Product does not have a collection of LoanItems
                .HasForeignKey(li => li.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting Product if LoanItems exist

            // LoanItem -> Loan (many-to-one)
            builder.HasOne(li => li.Loan)
                .WithMany(l => l.LoanItems)
                .HasForeignKey(li => li.LoanId)
                .OnDelete(DeleteBehavior.Cascade); // Delete LoanItems if Loan is deleted

            // Optional: add indexes for better performance
            builder.HasIndex(li => li.LoanId);
            builder.HasIndex(li => li.ProductId);
        }
    }
}
