using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (GetDefaultUser() == null)
            {
                var user = new ChronoUser("CoolName", "jan@wp.pl", "megamegaandrzej11");

                using (var dbContext = new ChronoDbContext())
                {
                    try
                    {
                        if (dbContext.Users.FirstOrDefault(x => x.UserName == user.UserName) == null)
                        {
                            dbContext.Users.Add(user);
                            dbContext.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                }
            }
            return View(GetDefaultUser());
        }

        public static ChronoUser GetDefaultUser()
        {
            ChronoUser user;

            using (var dbContext = new ChronoDbContext())
            {
                user = dbContext.Users.FirstOrDefault();
                if (user == null)
                {
                    return null;
                }
                user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();
            }
            return user;
        }
    }
}