using Domain.Entities.BranchReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.BranchReleated
{
    public sealed class BranchConfiguration :IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
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
            builder.Property(e => e.Latitude)
                .HasPrecision(23, 15);
            builder.Property(e => e.Longitude)
                .HasPrecision(24, 15);
            builder.Property(e => e.State)
                .HasMaxLength(255);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        }
    }
}
