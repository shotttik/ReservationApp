using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    internal sealed class WorkingExceptionConfiguration :IEntityTypeConfiguration<WorkScheduleException>
    {
        public void Configure(EntityTypeBuilder<WorkScheduleException> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.StartDateTime).IsRequired();
            builder.Property(e => e.EndDateTime).IsRequired();
            builder.Property(e => e.IsFullDay).IsRequired();
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasOne(e => e.Company)
                .WithMany(e => e.WorkScheduleExceptions)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.UserAccount)
                .WithMany(e => e.WorkScheduleExceptions)
                .HasForeignKey(e => e.UserAccountID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
