using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class WorkingScheduleConfiguration :IEntityTypeConfiguration<WorkSchedule>
    {
        public void Configure(EntityTypeBuilder<WorkSchedule> builder)
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
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

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
