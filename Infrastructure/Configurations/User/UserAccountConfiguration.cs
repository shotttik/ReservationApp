using Domain.Entities.User;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.User
{
    internal sealed class UserAccountConfiguration :IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Gender)
                .HasConversion<int>()
                .HasDefaultValue(Gender.PreferNotToSay);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.HasOne(ua => ua.UserLoginData)
                   .WithOne(uld => uld.UserAccount)
                   .HasForeignKey<UserAccount>(uld => uld.UserLoginDataID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
