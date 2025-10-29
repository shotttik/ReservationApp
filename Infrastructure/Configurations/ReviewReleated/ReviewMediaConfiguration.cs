using Domain.Entities.ReviewReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ReviewReleated
{
    public class ReviewMediaConfiguration :IEntityTypeConfiguration<ReviewMedia>
    {
        public void Configure(EntityTypeBuilder<ReviewMedia> builder)
        {
            builder.HasKey(cm => new { cm.ReviewId, cm.MediaId });
            builder.HasOne(cm => cm.Review)
                .WithMany(c => c.Media)
                .HasForeignKey(cm => cm.ReviewId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(cm => cm.Media)
                .WithMany(m => m.ReviewMedia)
                .HasForeignKey(cm => cm.MediaId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
