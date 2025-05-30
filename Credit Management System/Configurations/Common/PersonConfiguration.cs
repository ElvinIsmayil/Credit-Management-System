using Credit_Management_System.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Credit_Management_System.Configurations.Common
{
    public class PersonConfiguration<TPerson> : IEntityTypeConfiguration<TPerson> where TPerson : Person
    {
        public void Configure(EntityTypeBuilder<TPerson> builder)
        {
            // Key is inherited from BaseEntity (Id)
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Surname)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.BirthDate)
                .IsRequired()
                .HasColumnType("date"); // DateOnly maps well to date

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(255);

            builder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(p => p.Address)
                .HasMaxLength(250);
        }
    }
}
