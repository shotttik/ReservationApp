using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    internal sealed class WorkScheduleConfiguration :IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.DayOfWeek)
                .IsRequired();

            builder
                .Property(e => e.CreatedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);  // Ignore updates

            builder.HasOne(e => e.UserAccount)
                .WithMany(e => e.WorkSchedules)
                .HasForeignKey(e => e.UserAccountID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
