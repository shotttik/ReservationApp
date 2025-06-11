using Domain.Entities.LocationReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.LocationReleated
{
    public class StateConfiguration :IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(e => e.ID);

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.CountryId)
                .IsRequired();

            builder.Property(e => e.CountryCode)
                .HasMaxLength(2)
                .IsRequired();

            builder.Property(e => e.CountryName)
                .HasMaxLength(100);

            builder.Property(e => e.StateCode)
                .HasMaxLength(255);

            builder.Property(e => e.FipsCode)
                .HasMaxLength(255);

            builder.Property(e => e.Iso2)
                .HasMaxLength(255);

            builder.Property(e => e.Type)
                .HasMaxLength(191);

            builder.Property(e => e.Latitude)
                .HasPrecision(10, 8);

            builder.Property(e => e.Longitude)
                .HasPrecision(11, 8);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.WikiDataId)
                .HasMaxLength(255);

            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_State_Name");

            builder.HasOne(e => e.Country)
                .WithMany(c => c.States)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
