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
            return View(GetDefaultUser());
        }

        public static User GetDefaultUser()
        {
            User user;

            using (ChronoPillerDB dbContext = new ChronoPillerDB())
            {
                user = dbContext.Users.First();
                user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();
            }
            return user;
        }
    }
}