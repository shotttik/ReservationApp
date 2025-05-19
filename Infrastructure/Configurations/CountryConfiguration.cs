using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.ID);

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Iso3)
                .HasMaxLength(3);

            builder.Property(e => e.NumericCode)
                .HasMaxLength(3);

            builder.Property(e => e.Iso2)
                .HasMaxLength(2);

            builder.Property(e => e.PhoneCode)
                .HasMaxLength(255);

            builder.Property(e => e.Capital)
                .HasMaxLength(255);

            builder.Property(e => e.Currency)
                .HasMaxLength(255);

            builder.Property(e => e.CurrencyName)
                .HasMaxLength(255);

            builder.Property(e => e.CurrencySymbol)
                .HasMaxLength(255);

            builder.Property(e => e.Tld)
                .HasMaxLength(255);

            builder.Property(e => e.Native)
                .HasMaxLength(255);


            builder.Property(e => e.Nationality)
                .HasMaxLength(255);


            builder.Property(e => e.Latitude)
                .HasPrecision(10, 8);

            builder.Property(e => e.Longitude)
                .HasPrecision(11, 8);

            builder.Property(e => e.Emoji)
                .HasMaxLength(191);

            builder.Property(e => e.EmojiU)
                .HasMaxLength(191);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(e => e.Flag)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(e => e.WikiDataId)
                .HasMaxLength(255);
        }
    }
}
