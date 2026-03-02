using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    internal sealed class WorkScheduleExceptionConfiguration :IEntityTypeConfiguration<WorkScheduleException>
    {
        public void Configure(EntityTypeBuilder<WorkScheduleException> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.StartDate).IsRequired();
            builder.Property(e => e.EndDate).IsRequired();
            builder.Property(e => e.Type).IsRequired();
            builder.Property(e => e.Notes).HasMaxLength(2000);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasOne(e => e.UserAccount)
                .WithMany(e => e.WorkScheduleExceptions)
                .HasForeignKey(e => e.UserAccountID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
