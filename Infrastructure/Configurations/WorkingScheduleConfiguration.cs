using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class WorkingScheduleConfiguration :IEntityTypeConfiguration<WorkingSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkingSchedule> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.DayOfWeek)
                .IsRequired();
            builder.Property(e => e.IsWorkingDay).IsRequired();
            //builder.Property(e => e.StartTime)
            //    .HasConversion(
            //        v => v!.ToString(),
            //        v => TimeOnly.Parse(v))
            //    .IsRequired(false);
            builder.HasOne(e => e.Company)
                .WithMany(e => e.WorkingSchedules)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.User)
                .WithMany(e => e.WorkingSchedules)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
