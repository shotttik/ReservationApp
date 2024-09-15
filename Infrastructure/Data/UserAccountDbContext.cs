using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class UserAccountDbContext :DbContext
    {
        public UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : base(options)
        {

        }
        public DbSet<UserAccount> UserAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserAccount>().ToTable("UserAccount");
        }
    }
}
