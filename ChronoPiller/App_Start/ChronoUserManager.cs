using ChronoPiller.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace ChronoPiller.Models
{
    public class ChronoUserManager : UserManager<ChronoUser, int>
    {
        public ChronoUserManager(IUserStore<ChronoUser, int> store) : base(store)
        {
        }

        public static ChronoUserManager Create(IdentityFactoryOptions<ChronoUserManager> options, IOwinContext context)
        {
            var manager = new ChronoUserManager(new ChronoUserStore(context.Get<ChronoDbContext>()));
            manager.UserValidator = new UserValidator<ChronoUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false
            };
            // Register two factor authentication providers. This application uses Phone 
            // and Emails as a step of receiving a code for verifying the user 
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ChronoUser, int>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ChronoUser, int>
            {
                Subject = "ChronoPiller security code",
                BodyFormat = "Your security code is {0}"
            });

//            manager.EmailService = new EmailService();
//            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;

            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ChronoUser, int>(
                    dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }
    }

    public class ChronoSignInManager : SignInManager<ChronoUser, int>
    {
        public ChronoSignInManager(UserManager<ChronoUser, int> userManager, IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }
    }
}