using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserApplication.Models;
using UserApplication.Areas.Administration.Models;

namespace UserApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<UserApplication.Areas.Administration.Models.UserModel> UserModel { get; set; } = default!;
    }
}
