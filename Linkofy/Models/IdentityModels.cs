using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Linkofy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer<ApplicationDbContext>(new LinksDBInitialiser());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();

        }
        public DbSet<Identifier> Identifiers { get; set; }
        public DbSet<UserTable> UserTables { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Status> Statuss { get; set; }
        public DbSet<MJTopics> MJTopicss { get; set; }
        public DbSet<MajesticData> MajesticDatas { get; set; }
        public DbSet<DataTables> DataTabless { get; set; }
        public DbSet<Results> Resultss { get; set; }
        public DbSet<Headers> Headerss { get; set; }
    }
    }