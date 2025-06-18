using Domain.Entities.CompanyReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class CompanyMediaConfiguration :IEntityTypeConfiguration<CompanyMedia>
    {
        public void Configure(EntityTypeBuilder<CompanyMedia> builder)
        {
            builder.HasKey(cm => new { cm.CompanyID, cm.MediaID });
            builder.HasOne(cm => cm.Company)
                .WithMany(c => c.CompanyMedias)
                .HasForeignKey(cm => cm.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cm => cm.Media)
                .WithMany(m => m.CompanyMedias)
                .HasForeignKey(cm => cm.MediaID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
