using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Common
{
    public class MediaConfiguration :IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(e => e.FilePath)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.FileSize)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
        }
    }
}
