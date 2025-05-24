using Credit_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            // First Name
            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            // Last Name
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            // ImageUrl (optional)
            builder.Property(u => u.ImageUrl)
                .HasMaxLength(255);

            // CreatedDate
            builder.Property(u => u.CreatedDate)
                .IsRequired();

            // LastLoginDate
            builder.Property(u => u.LastLoginDate)
                .IsRequired(false);

            // IsActive
            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);
        }
    }
}
