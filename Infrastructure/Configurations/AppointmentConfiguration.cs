using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class AppointmentConfiguration :IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(e => e.ID);
            builder.Property(e => e.StartTime).IsRequired();
            builder.Property(e => e.EndTimeExpected).IsRequired();
            builder.Property(e => e.PriceExpected).IsRequired();
            builder.Property(e => e.Discount).HasPrecision(18, 2);
            builder.Property(e => e.PriceFinal).HasPrecision(18, 2);
            builder.Property(e => e.PriceFull).HasPrecision(18, 2);

            builder.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(AppointmentStatus.Pending);
            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAdd();
            builder.HasOne(e => e.Client)
                .WithMany(e => e.AppointmentsAsClient)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Employee)
                .WithMany(e => e.AppointmentsAsEmployee)
                .HasForeignKey(e => e.EmployeeID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Company)
                .WithMany(e => e.Appointments)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
