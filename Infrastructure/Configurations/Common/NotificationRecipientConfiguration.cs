using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class NotificationRecipientConfiguration :IEntityTypeConfiguration<NotificationRecipient>
    {
        public void Configure(EntityTypeBuilder<NotificationRecipient> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Notification)
                .WithMany(n => n.Recipients)
                .HasForeignKey(e => e.NotificationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UserAccount)
                .WithMany(u => u.NotificationRecipients)
                .HasForeignKey(e => e.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.DeliveryStatus)
            .HasConversion<int>()
            .IsRequired();

            builder.Property(e => e.LastDeliveryError)
                .HasMaxLength(2000);

            builder.HasIndex(e => new { e.NotificationId, e.UserAccountId, e.DeliveryStatus, e.DeliveryAttempts, e.CreatedAt });
        }
    }
}
