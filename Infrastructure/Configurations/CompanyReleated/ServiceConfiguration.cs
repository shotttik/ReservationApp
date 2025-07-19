using Domain.Entities.CompanyReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    internal sealed class ServiceConfiguration :IEntityTypeConfiguration<Service>
    {
        public void Configure(EntityTypeBuilder<Service> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Name).HasMaxLength(255).IsRequired();
            builder.HasIndex(e => e.Name).IsUnique();
            builder.Property(e => e.Duration).IsRequired();
            builder.Property(e => e.Price).HasPrecision(18, 2);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.HasOne(e => e.Company)
                .WithMany(e => e.Services)
                .HasForeignKey(e => e.CompanyID);
        }
    }
}
