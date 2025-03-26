using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserLoginDataConfiguration :IEntityTypeConfiguration<UserLoginData>
    {
        public void Configure(EntityTypeBuilder<UserLoginData> builder)
        {
            builder.Property(e => e.Email).IsRequired().HasMaxLength(255);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(e => e.ConfirmationToken).HasMaxLength(150);
            builder.Property(e => e.PasswordRecoveryToken).HasMaxLength(150);
            builder.Property(e => e.VerificationToken).HasMaxLength(150);
            builder.Property(e => e.UserAccountID).IsRequired();
            builder.Property(e => e.VerificationStatus)
                    .HasConversion<int>()
                    .HasDefaultValue(VerificationStatus.Unverified); ; // deactivated
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}
