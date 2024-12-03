using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class RoleConfiguration :IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {

            builder.HasKey(x => x.ID);

            builder.Property(x => x.Name)
                   .IsRequired();

            builder.HasMany(x => x.Permissions)
                   .WithMany()
                   .UsingEntity<RolePermission>();

            builder.HasMany(x => x.UserAccounts)
                   .WithMany(x => x.Roles);

            builder.HasData(Role.GetValues());
        }
    }
}
