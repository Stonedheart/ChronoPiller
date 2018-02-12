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
        }
    }
}