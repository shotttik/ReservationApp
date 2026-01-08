using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class BookingVerificationConfiguration :IEntityTypeConfiguration<BookingVerification>
    {
        public void Configure(EntityTypeBuilder<BookingVerification> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.VerificationType)
                .IsRequired()
                .HasConversion<int>();
            builder.Property(e => e.CodeHash)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(bv => bv.Booking)
                .WithMany(b => b.Verifications)
                .HasForeignKey(e => e.BookingId)
                .IsRequired();

            builder.HasIndex(g => new { g.VerificationType });
        }
    }
}
