using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Owin.Security.Providers.Orcid.Message;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (GetDefaultUser() == null)
            {

                using (var dbContext = new ChronoDbContext())
                {
                    try
                    {

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