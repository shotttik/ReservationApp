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
        public DbSet<HashingAlgorithm> HashingAlgorithms { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>()
                .HasOne(ua => ua.UserLoginData)
                .WithOne(uld => uld.UserAccount)
                .HasForeignKey<UserLoginData>(uld => uld.UserAccountID)
                .IsRequired();

            modelBuilder.Entity<HashingAlgorithm>()
                 .HasMany(ha => ha.UserLoginDatas)
                 .WithOne(uld => uld.HashingAlgorithm)
                 .HasForeignKey(uld => uld.HashAlgorithmID)
                 .IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasMany(ur => ur.Permissions)
                .WithMany(p => p.UserRoles);

            modelBuilder.Entity<UserRole>()
                .HasMany(ur => ur.UserAccounts)
                .WithOne(ua => ua.Role)
                .HasForeignKey(ua => ua.RoleID)
                .IsRequired();
        }
    }
}
