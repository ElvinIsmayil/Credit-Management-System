using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Credit_Management_System.Configurations
{
    public class LoanItemConfiguration : IEntityTypeConfiguration<LoanItem>
    {
        public void Configure(EntityTypeBuilder<LoanItem> builder)
        {
            builder.Property(li => li.UnitPrice).HasPrecision(18, 2);

            builder.HasOne(li => li.Product)
                   .WithMany()
                   .HasForeignKey(li => li.ProductId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(li => li.Loan)
                   .WithMany(l => l.LoanItems)
                   .HasForeignKey(li => li.LoanId)
                   .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}
