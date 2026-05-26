using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class PromoCodeConfiguration :IEntityTypeConfiguration<PromoCode>
    {
        public void Configure(EntityTypeBuilder<PromoCode> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Code)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(p => p.Company)
                .WithMany(c => c.PromoCodes)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(p => p.Bookings)
                .WithOne(b => b.PromoCode)
                .HasForeignKey(b => b.PromoCodeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}
