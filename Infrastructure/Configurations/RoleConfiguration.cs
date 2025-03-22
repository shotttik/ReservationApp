using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class RoleConfiguration :IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.HasKey(e => e.ID);

            builder.Property(e => e.Name)
                   .IsRequired();

            builder.HasMany(e => e.Permissions)
                   .WithMany()
                   .UsingEntity<RolePermission>();

            builder.HasMany(e => e.UserAccounts)
                   .WithOne(e => e.Role)
                   .HasForeignKey(e => e.RoleID);

            builder.HasData(Role.GetValues());
        }
    }
}
