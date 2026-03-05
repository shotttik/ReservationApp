using Domain.Entities.CompanyReleated;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class CompanySubscriptionConfiguration :IEntityTypeConfiguration<CompanySubscription>

    {
        public void Configure(EntityTypeBuilder<CompanySubscription> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(SubscriptionStatus.Active);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(cs => cs.CompanyId).IsUnique();

            builder.HasOne(cs => cs.Company)
                .WithOne(c => c.Subscription)
                .HasForeignKey<CompanySubscription>(cs => cs.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cs => cs.SubscriptionPlan)
                .WithMany(sp => sp.CompanySubscriptions)
                .HasForeignKey(cs => cs.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
