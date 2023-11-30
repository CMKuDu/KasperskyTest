using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestTelcoHub.Model.Model;

namespace TestTelcoHub.Model.Data
{
    public class ApiDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> ops) : base(ops) { }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Distributor> Distributors { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<Expiration> Expirations { get; set; }
        public virtual DbSet<ExternalReference> ExternalReferences { get; set; }
        public virtual DbSet<TermsAndConditions> TermsAndConditions { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }
        public virtual DbSet<ApprovalCode> ApprovalCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedRoles(modelBuilder);
            modelBuilder.Entity<Contacts>();
            modelBuilder.Entity<Exception>().HasNoKey();

        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                new IdentityRole() { Name = "Customer", ConcurrencyStamp = "2", NormalizedName = "Customer" },
                new IdentityRole() { Name = "User", ConcurrencyStamp = "3", NormalizedName = "User" }
                );
        }
    }

}
