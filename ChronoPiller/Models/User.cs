using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using ChronoPiller.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChronoPiller.Models
{
    public class ChronoUserRole : IdentityUserRole<int>
    {
    }

    public class ChronoUserClaim : IdentityUserClaim<int>
    {
    }

    public class ChronoUserLogin : IdentityUserLogin<int>
    {
    }

    public class ChronoRole : IdentityRole<int, ChronoUserRole>
    {
        public ChronoRole()
        {
        }

        public ChronoRole(string name)
        {
            Name = name;
        }
    }

    public class
        ChronoUserStore : UserStore<ChronoUser, ChronoRole, int, ChronoUserLogin, ChronoUserRole, ChronoUserClaim>
    {
        public ChronoUserStore(ChronoDbContext context) : base(context)
        {
        }
    }

    public class ChronoRoleStore : RoleStore<ChronoRole, int, ChronoUserRole>
    {
        public ChronoRoleStore(ChronoDbContext context) : base(context)
        {
        }
    }


    public class ChronoUser : IdentityUser<int, ChronoUserLogin, ChronoUserRole,
        ChronoUserClaim>
    {
        [Key]
        public override int Id { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        public List<Prescription> Prescriptions { get; set; }

        public ChronoUser()
        {
        }


        public ChronoUser(string name, string email, string password)
        {
            UserName = name;
            Email = email;
            Password = password;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ChronoUser, int> manager)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(
                this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}