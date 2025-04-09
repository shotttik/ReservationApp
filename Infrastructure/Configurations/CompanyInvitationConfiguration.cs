using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    internal sealed class CompanyInvitationConfiguration :IEntityTypeConfiguration<CompanyInvitation>
    {
        public void Configure(EntityTypeBuilder<CompanyInvitation> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.Property(e => e.UserAccountID).IsRequired();
            builder.Property(e => e.Token).HasMaxLength(150);
            builder.HasIndex(e => e.Token).IsUnique();

            builder.HasOne(e => e.Company)
                .WithMany(e => e.Invitations)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
