using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class LoanDetailConfiguration : IEntityTypeConfiguration<LoanDetail>
    {
        public void Configure(EntityTypeBuilder<LoanDetail> builder)
        {
            // Primary key from BaseEntity (Id)
            builder.HasKey(ld => ld.Id);

            // Properties
            builder.Property(ld => ld.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ld => ld.AmountPaid)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Ignore(ld => ld.RemainingAmount); // Computed property, not mapped

            builder.Property(ld => ld.InterestRate)
                .IsRequired()
                .HasColumnType("decimal(5,2)");

            builder.Property(ld => ld.LoanDate)
                .IsRequired();

            builder.Property(ld => ld.DueDate)
                .IsRequired();

            // Relationships

            builder.HasOne(ld => ld.Loan)
                .WithOne(l => l.LoanDetail)  // Assuming Loan has navigation property LoanDetail
                .HasForeignKey<LoanDetail>(ld => ld.LoanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
