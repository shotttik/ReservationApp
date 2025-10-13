using Domain.Entities.ReviewReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.ReviewReleated
{
    public class ReviewInviteConfiguration :IEntityTypeConfiguration<ReviewInvite>
    {
        public void Configure(EntityTypeBuilder<ReviewInvite> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.HasOne(rv => rv.Booking)
                .WithOne(b => b.ReviewInvite)
                .HasForeignKey<ReviewInvite>(rv => rv.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
