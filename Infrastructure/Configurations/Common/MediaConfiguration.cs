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
            builder.Property(e => e.OriginalName)
                .IsRequired()
                .HasMaxLength(255);
            builder.Property(e => e.RemoteUrl)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(e => e.OriginalUrl)
                .IsRequired()
                .HasMaxLength(2000);
            builder.Property(e => e.FileType)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(e => e.FileSizeInBytes)
                .IsRequired();
            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            builder.HasIndex(e => e.RemoteUrl);
            builder.HasIndex(e => e.OriginalUrl);
        }
    }
}
