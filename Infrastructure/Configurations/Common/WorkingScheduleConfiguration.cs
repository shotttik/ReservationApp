using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    internal sealed class WorkingScheduleConfiguration :IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.DayOfWeek)
                .IsRequired();
            builder.Property(e => e.IsWorkingDay).IsRequired();

            builder
                .Property(e => e.CreatedAt)
                .ValueGeneratedOnAdd() 
                .HasDefaultValueSql("GETDATE()")
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);  // Ignore updates

            builder.HasOne(e => e.Company)
                .WithMany(e => e.WorkSchedules)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.User)
                .WithMany(e => e.WorkSchedules)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
