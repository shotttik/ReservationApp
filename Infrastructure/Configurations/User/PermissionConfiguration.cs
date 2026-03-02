using Domain.Entities.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.User
{
    internal sealed class PermissionConfiguration :IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.HasKey(p => p.Id);

            IEnumerable<Permission> permissions = Enum
                .GetValues<Domain.Enums.Permission>()
                .Select(p => new Permission
                {
                    Id = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(permissions);
        }
    }
}
