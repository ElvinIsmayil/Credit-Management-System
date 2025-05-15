using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            // Primary key from BaseEntity (Id)
            builder.HasKey(b => b.Id);

            // Properties
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(b => b.Address)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(b => b.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // Relationships

            // Branch belongs to one Merchant; Merchant has many Branches
            builder.HasOne(b => b.Merchant)
                .WithMany(m => m.Branches)
                .HasForeignKey(b => b.MerchantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Branch has many Employees
            builder.HasMany(b => b.Employees)
                .WithOne(e => e.Branch)
                .HasForeignKey(e => e.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Optional indexes for optimization
            builder.HasIndex(b => b.Name);
            builder.HasIndex(b => b.PhoneNumber);
        }
    }
}
