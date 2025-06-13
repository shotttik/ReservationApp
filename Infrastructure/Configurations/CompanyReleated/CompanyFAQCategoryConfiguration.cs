using Domain.Entities.CompanyReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class CompanyFAQCategoryConfiguration :IEntityTypeConfiguration<CompanyFAQCategory>
    {
        public void Configure(EntityTypeBuilder<CompanyFAQCategory> builder)
        {
            builder.HasKey(c => c.ID);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasIndex(c => c.Name);

            builder.HasMany(c => c.FAQs)
                .WithOne(f => f.Category)
                .HasForeignKey(f => f.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Company)
                .WithMany(c => c.CompanyFAQCategories)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
