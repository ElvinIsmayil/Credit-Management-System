using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class MerchantConfiguration : IEntityTypeConfiguration<Merchant>
    {
        public void Configure(EntityTypeBuilder<Merchant> builder)
        {
            // Primary key inherited from BaseEntity (Id)
            builder.HasKey(m => m.Id);

            // Properties
            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(m => m.Adress)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(m => m.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.ImageUrl)
                .HasMaxLength(255);

            // Relationships

            // One Merchant has many Branches
            builder.HasMany(m => m.Branches)
                .WithOne(b => b.Merchant)
                .HasForeignKey(b => b.MerchantId)
                .OnDelete(DeleteBehavior.Cascade);

            // Optional: Indexes if needed
            builder.HasIndex(m => m.Name);
            builder.HasIndex(m => m.Email).IsUnique();
        }
    }
}
