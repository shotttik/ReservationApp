using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class PermissionConfiguration :IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {

            builder.HasKey(p => p.ID);

            IEnumerable<Permission> permissions = Enum
                .GetValues<Domain.Enums.Permission>()
                .Select(p => new Permission
                {
                    ID = (int)p,
                    Name = p.ToString()
                });

            builder.HasData(permissions);
        }
    }
}
