using Domain.Entities;
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
            builder.Property(e => e.UserAccountID).IsRequired();
            builder.Property(e => e.EmailValidationStatus).HasDefaultValue(0); // deactivated
            builder.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
        }
    }
}
