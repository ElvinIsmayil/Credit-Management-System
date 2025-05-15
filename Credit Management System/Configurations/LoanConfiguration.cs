using Credit_Management_System.Models;
using Credit_Management_System.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            // Primary key from BaseEntity (Id)
            builder.HasKey(l => l.Id);

            // Properties
            builder.Property(l => l.LoanStatus)
                .IsRequired()
                .HasConversion<string>(); // Store enum as string (optional, remove if prefer int)

            builder.Property(l => l.CreatedDate)
                .IsRequired();

            builder.Property(l => l.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Relationships

            // Loan belongs to one Customer; Customer can have many Loans
            builder.HasOne(l => l.Customer)
                .WithMany(c => c.Loans)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Loan optionally assigned to one Employee; Employee can have many Loans
            builder.HasOne(l => l.Employee)
                .WithMany(e => e.ManagedLoans)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);

            // One-to-one LoanDetail
            builder.HasOne(l => l.LoanDetail)
                .WithOne(ld => ld.Loan)
                .HasForeignKey<LoanDetail>(ld => ld.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many Payments
            builder.HasMany(l => l.Payments)
                .WithOne(p => p.Loan)
                .HasForeignKey(p => p.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many LoanItems
            builder.HasMany(l => l.LoanItems)
                .WithOne(li => li.Loan)
                .HasForeignKey(li => li.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
