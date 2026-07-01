using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class OutboxMessageConfiguration :IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Type)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.PayloadJson)
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(e => e.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(e => e.LastError)
                .HasMaxLength(2000);

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();

            builder.HasIndex(e => new { e.Status, e.Attempts, e.CreatedAt });
        }
    }
}
