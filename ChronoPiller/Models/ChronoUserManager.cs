using ChronoPiller.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace ChronoPiller.Models
{
    public class ChronoUserManager : UserManager<ChronoUser, int>
    {
        public ChronoUserManager(IUserStore<ChronoUser, int> store) : base(store)
        {
        }

        public static ChronoUserManager Create(IdentityFactoryOptions<ChronoUserManager> options, IOwinContext context)
        {
            var manager = new ChronoUserManager(new ChronoUserStore(context.Get<ChronoPillerDb>()));
            manager.UserValidator = new UserValidator<ChronoUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
        }
    }
}