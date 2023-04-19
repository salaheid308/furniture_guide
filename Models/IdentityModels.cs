using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Dalelk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string usertype { get; set; }
       
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
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Dalelk.Models.category> categories { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.product> products { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.productimages> productimages { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.gallries> gallries { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.welcomepage> welcomepages { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.productcolor > productcolor { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.productcolor2> productcolor2 { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.productprice> productprice { get; set; }

       

        public System.Data.Entity.DbSet<Dalelk.Models.productsize> productsize { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.city> cities { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.storeowner> storeowners { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.plogs> plogs { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.comments> comments { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.Replay> Replays { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.notification> notifications { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.drivers> drivers { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.carimages> carimages { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.driverprice> driverprices { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.messages> messages { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.productrate> productrates { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.cart> carts { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.favorite> favorites { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.order> orders { get; set; }

        public System.Data.Entity.DbSet<Dalelk.Models.vefycods> vefycods { get; set; }
        public System.Data.Entity.DbSet<Dalelk.Models.buyer> buyers { get; set; }

    }
}