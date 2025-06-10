using Domain.Entities.LocationReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.LocationReleated
{
    public sealed class CityConfiguration :IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(e => e.ID);

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.StateCode)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(e => e.Latitude)
                .HasPrecision(10, 8)
                .IsRequired();

            builder.Property(e => e.Longitude)
                .HasPrecision(11, 8)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate()
                .IsRequired();

            builder.Property(e => e.Flag)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(e => e.WikiDataId)
                .HasMaxLength(255);

            // Relationships
            builder.HasOne(e => e.Country)
                .WithMany(e => e.Cities)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
