using Domain.Entities.User;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.User
{
    internal sealed class UserLoginDataConfiguration :IEntityTypeConfiguration<UserLoginData>
    {
        public void Configure(EntityTypeBuilder<UserLoginData> builder)
        {
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.Phone).HasMaxLength(15);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(e => e.RecoveryToken).HasMaxLength(150);
            builder.Property(e => e.EmailVerificationToken).HasMaxLength(150);
            builder.Property(e => e.PhoneVerificationToken).HasMaxLength(150);
            builder.Property(e => e.PendingNewEmail).HasMaxLength(255);
            builder.Property(e => e.PendingNewPhone).HasMaxLength(15);
            builder.Property(e => e.EmailVerificationStatus)
                    .HasConversion<int>()
                    .HasDefaultValue(VerificationStatus.Unverified);
            builder.Property(e => e.PhoneVerificationStatus)
                    .HasConversion<int>()
                    .HasDefaultValue(VerificationStatus.Unverified);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.PendingNewEmail).IsUnique();
        }
    }
}
