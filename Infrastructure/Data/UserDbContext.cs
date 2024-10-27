using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserDbContext :DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserLoginData> UserLoginDatas { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.UserLoginData)
                .WithOne(uld => uld.UserAccount)
                .HasForeignKey<UserLoginData>(uld => uld.UserAccountID)
                .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasMany(ur => ur.Permissions)
                .WithMany(p => p.UserRoles);

            modelBuilder.Entity<UserRole>()
                .HasMany(ur => ur.UserAccounts)
                .WithOne(ua => ua.Role)
                .HasForeignKey(ua => ua.RoleID)
                .IsRequired();

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.Property(e => e.PermissionDescription).HasMaxLength(50).IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.RoleDescription).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<UserAccount>(
                entity =>
                {
                    entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                    entity.Property(e => e.Gender).HasDefaultValue((int)Application.Enums.Gender.PreferNotToSay).ValueGeneratedOnAdd();
                    entity.Property(e => e.RoleID).HasDefaultValue(1); // User
                    entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();
                    entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
                }
            );

            modelBuilder.Entity<UserLoginData>(entity =>
            {
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(e => e.ConfirmationToken).HasMaxLength(150);
                entity.Property(e => e.PasswordRecoveryToken).HasMaxLength(150);
                entity.Property(e => e.UserAccountID).IsRequired();
                entity.Property(e => e.EmailValidationStatus).HasDefaultValue(0); // deactivated
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            });
        }
    }
}
