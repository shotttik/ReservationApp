using Domain.Entities.CompanyReleated;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class CompanyConfiguration :IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.Property(e => e.Description).HasColumnType("nvarchar(max)");
            builder.Property(e => e.IN).HasMaxLength(9).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.Phone).HasMaxLength(9);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.Property(e => e.Type)
                .HasConversion<int>()
                .HasDefaultValue(CompanyType.None);

            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasIndex(e => e.IN).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.Phone).IsUnique();

            builder.HasMany(c => c.UserAccounts)
                .WithOne(ua => ua.Company)
                .HasForeignKey(ua => ua.CompanyID)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(c => c.Location)
                .WithOne(l => l.Company)
                .HasForeignKey<Company>(c => c.LocationID)
                .IsRequired();
        }
    }
}
