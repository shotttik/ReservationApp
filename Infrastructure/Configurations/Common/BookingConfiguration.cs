using Domain.Entities.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class BookingConfiguration :IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTimeExpected).IsRequired();
            builder.Property(e => e.PriceExpected).IsRequired();
            builder.Property(e => e.Discount).HasPrecision(18, 2);
            builder.Property(e => e.PriceFinal).HasPrecision(18, 2);
            builder.Property(e => e.PriceFull).HasPrecision(18, 2);
            builder.Property(e => e.CancellationReason)
                .HasMaxLength(2000);
            builder.Property(e => e.Note)
                    .HasMaxLength(2000);
            builder.Property(e => e.ServiceName)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(BookingStatus.Pending);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            builder.HasOne(e => e.Client)
                .WithMany(e => e.BookingsAsClient)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.BookingsAsEmployee)
                .HasForeignKey(e => e.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Company)
                .WithMany(e => e.Bookings)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
