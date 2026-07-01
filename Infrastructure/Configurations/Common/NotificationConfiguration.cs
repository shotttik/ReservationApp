using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class NotificationConfiguration :IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TargetType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.Type)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(e => e.DataJson)
                .HasColumnType("nvarchar(max)");

            builder.Property(e => e.DeliveryStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.LastDeliveryError)
                .HasMaxLength(2000);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(e => new { e.TargetType, e.TargetId, e.ReadAt });
            builder.HasIndex(e => new { e.DeliveryStatus, e.DeliveryAttempts, e.CreatedAt });
        }
    }
}
