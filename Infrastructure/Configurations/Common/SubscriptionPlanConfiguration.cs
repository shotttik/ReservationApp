using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class SubscriptionPlanConfiguration :IEntityTypeConfiguration<SubscriptionPlan>
    {
        public void Configure(EntityTypeBuilder<SubscriptionPlan> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(e => e.Name)
                .IsUnique();
            builder.Property(e => e.PriceMonthly)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        }
    }
}
