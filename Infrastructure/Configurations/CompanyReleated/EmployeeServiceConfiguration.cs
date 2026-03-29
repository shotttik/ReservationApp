using Domain.Entities.CompanyReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class EmployeeServiceConfiguration :IEntityTypeConfiguration<EmployeeService>
    {
        public void Configure(EntityTypeBuilder<EmployeeService> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Employee)
                .WithMany(x => x.EmployeeServices)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Service)
                .WithMany(x => x.EmployeeServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => new { x.EmployeeId, x.ServiceId })
                .IsUnique();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd()
                .IsRequired();
        }
    }
}
