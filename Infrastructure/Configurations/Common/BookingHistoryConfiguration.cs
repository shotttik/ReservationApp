using Domain.Entities.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class BookingHistoryConfiguration :IEntityTypeConfiguration<BookingHistory>
    {
        public void Configure(EntityTypeBuilder<BookingHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ActionType)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Source)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.ChangesJson)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(x => new
            {
                x.BookingId,
                x.ChangedByUserId,
                x.CreatedAt
            });

            builder.HasOne(x => x.Booking)
                .WithMany(x => x.History)
                .HasForeignKey(x => x.BookingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
