using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.User
{
    public class UserAccountMediaConfiguration :IEntityTypeConfiguration<UserAccountMedia>
    {
        public void Configure(EntityTypeBuilder<UserAccountMedia> builder)
        {
            builder.HasKey(e => new { e.UserAccountId, e.MediaId });
            builder.HasOne(e => e.UserAccount)
                .WithMany(ua => ua.UserAccountMedias)
                .HasForeignKey(e => e.UserAccountId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Media)
                .WithMany(m => m.UserAccountMedias)
                .HasForeignKey(e => e.MediaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
