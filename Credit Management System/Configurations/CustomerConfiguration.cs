using Credit_Management_System.Configurations.Common;
using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // Apply base Person configuration
            new PersonConfiguration<Customer>().Configure(builder);

            // PostalCode - optional, max length constraint
            builder.Property(c => c.PostalCode)
                .HasMaxLength(20);

            // CreditLimit - required with proper decimal precision
            builder.Property(c => c.CreditLimit)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Relationships

            // Customer has many Loans
            builder.HasMany(c => c.Loans)
                .WithOne(l => l.Customer)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
