using Domain.Entities.ReviewReleated;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ReviewReleated
{
    public class ReviewConfiguration :IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.Status)
                .HasConversion<int>()
                .HasDefaultValue(ReviewStatus.Pending);
            builder.Property(e => e.Body)
                .HasMaxLength(2000);
            builder.Property(e => e.Overall)
                .IsRequired();

            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();

            builder.HasOne(e => e.ReviewInvite)
                .WithOne(e => e.Review)
                .HasForeignKey<Review>(c => c.ReviewInviteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
