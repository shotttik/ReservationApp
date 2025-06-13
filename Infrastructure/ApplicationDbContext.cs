using Domain.Entities.Common;
using Domain.Entities.CompanyReleated;
using Domain.Entities.LocationReleated;
using Domain.Entities.User;
using Infrastructure.Configurations.Common;
using Infrastructure.Configurations.CompanyReleated;
using Infrastructure.Configurations.LocationReleated;
using Infrastructure.Configurations.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserLoginData> UserLoginDatas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyInvitation> CompanyInvitations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<WorkScheduleException> WorkScheduleExceptions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<CompanyFAQ> CompanyFAQs { get; set; }
        public DbSet<CompanyFAQCategory> CompanyFAQCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User Releated
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginDataConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());

            // Company Releated
            modelBuilder.ApplyConfiguration(new CompanyConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyInvitationConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFAQConfiguration());
            modelBuilder.ApplyConfiguration(new CompanyFAQCategoryConfiguration());

            // Common
            modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new WorkingExceptionConfiguration());

            // Location Releated
            modelBuilder.ApplyConfiguration(new CountryConfiguration());
            modelBuilder.ApplyConfiguration(new CityConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
        }
    }
}
