using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.LocationReleated
{
    public sealed class LocationConfiguration :IEntityTypeConfiguration<Domain.Entities.LocationReleated.Location>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.LocationReleated.Location> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.AddressLine1)
                .HasMaxLength(255);
            builder.Property(e => e.AddressLine2)
                .HasMaxLength(255);
            builder.Property(e => e.City)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(e => e.PostalCode)
                .HasMaxLength(20);
            builder.Property(e => e.Country)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(e => e.State)
                .HasMaxLength(255);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        }
    }
}
