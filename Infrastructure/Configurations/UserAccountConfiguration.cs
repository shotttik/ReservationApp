using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class UserAccountConfiguration :IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            builder.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.LastName).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Gender).HasDefaultValue((int)Application.Enums.Gender.PreferNotToSay).ValueGeneratedOnAdd();
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasOne(ua => ua.UserLoginData)
                   .WithOne(uld => uld.UserAccount)
                   .HasForeignKey<UserLoginData>(uld => uld.UserAccountID)
                   .IsRequired();
        }
    }
}
