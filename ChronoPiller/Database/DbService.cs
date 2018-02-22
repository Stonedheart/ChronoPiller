using System.Web;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ChronoPiller.Database
{
    public class DbService
    {
        public ChronoUser User { get; set; }

        public DbService()
        {
            User = GetCurrentUser();
        }

        private ChronoUser GetCurrentUser()
        {
            var context = HttpContext.Current;
            var id = context.User.Identity.GetUserId();
            return context.GetOwinContext().GetUserManager<ChronoUserManager>().FindById(int.Parse(id));
        }
    }
}