using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class BookingGuestInfoConfiguration :IEntityTypeConfiguration<BookingGuestInfo>
    {
        public void Configure(EntityTypeBuilder<BookingGuestInfo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ContactType)
                .IsRequired()
                .HasConversion<int>();
            builder.Property(e => e.Contact)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(e => e.DisplayName)
                .HasMaxLength(100);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasOne(bgi => bgi.Booking)
                .WithOne(b => b.GuestInfo)
                .HasForeignKey<BookingGuestInfo>(bgi => bgi.BookingId)
                .IsRequired();

            builder.HasIndex(g => g.BookingId).IsUnique();
            builder.HasIndex(g => new { g.ContactType, g.Contact });
        }
    }
}
