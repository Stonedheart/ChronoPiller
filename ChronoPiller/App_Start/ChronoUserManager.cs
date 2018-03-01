using System;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace ChronoPiller
{
    public class ChronoEmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message) => configureSendMailAsync(message);

        private async Task configureSendMailAsync(IdentityMessage message)
        {
            var myMessage = new MailMessage();
            myMessage.To.Add(message.Destination);
            myMessage.From = new MailAddress("chronopiller@gmail.com", "ChronoPiller Team");
            myMessage.Subject = message.Subject;
            myMessage.Body = message.Body;
            myMessage.IsBodyHtml = true;

            var client = new DefaultEmailClient("chronopiller@gmail.com",
                "dupadupadupa");

            await client.SendMailAsync(myMessage);
        }
    }

    public class ChronoUserManager : UserManager<ChronoUser, int>
    {
        public ChronoUserManager(IUserStore<ChronoUser, int> store) : base(store)
        {
        }

        public static ChronoUserManager Create(IdentityFactoryOptions<ChronoUserManager> options, IOwinContext context)
        {
            var manager = new ChronoUserManager(
                new UserStore<ChronoUser, ChronoRole, int, ChronoUserLogin, ChronoUserRole, ChronoUserClaim>(context.Get<ChronoDbContext>()));
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
            manager.UserLockoutEnabledByDefault = false;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

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

            manager.EmailService = new ChronoEmailService();
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
        public ChronoSignInManager(UserManager<ChronoUser, int> userManager,
            IAuthenticationManager authenticationManager) : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ChronoUser user)
        {
            return user.GenerateUserIdentityAsync((ChronoUserManager) UserManager);
        }

        public static ChronoSignInManager Create(IdentityFactoryOptions<ChronoSignInManager> options,
            IOwinContext context)
        {
            return new ChronoSignInManager(context.GetUserManager<ChronoUserManager>(), context.Authentication);
        }
    }
}