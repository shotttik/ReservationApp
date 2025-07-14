using Domain.Entities.CompanyReleated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.CompanyReleated
{
    public class CompanyFAQConfiguration :IEntityTypeConfiguration<CompanyFAQ>

    {
        public void Configure(EntityTypeBuilder<CompanyFAQ> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.ID)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(e => e.Question)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(e => e.Answer)
                .IsRequired()
                .HasMaxLength(2000);
        }
    }
}
