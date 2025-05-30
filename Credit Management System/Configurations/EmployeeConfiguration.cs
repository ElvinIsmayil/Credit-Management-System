using Credit_Management_System.Configurations.Common;
using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            // Apply configuration for inherited Person properties
            new PersonConfiguration<Employee>().Configure(builder);

            // Configure Position enum stored as string for readability
            
            // Salary configuration
            builder.Property(e => e.Salary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Relationship: Employee belongs to one Branch
            builder.HasOne(e => e.Branch)
                .WithMany(b => b.Employees)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Employee manages many Loans
            builder.HasMany(e => e.ManagedLoans)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
