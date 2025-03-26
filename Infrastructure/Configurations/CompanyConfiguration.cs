using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CompanyConfiguration :IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Description).HasMaxLength(255);
            builder.Property(e => e.IN).HasMaxLength(9).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.Phone).HasMaxLength(9);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasIndex(e => e.IN).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.Phone).IsUnique();

            builder.HasMany(c => c.UserAccounts)
                .WithOne(ua => ua.Company)
                .HasForeignKey(ua => ua.CompanyID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
