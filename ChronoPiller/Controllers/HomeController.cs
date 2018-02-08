using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dbContext = new ChronoPillerDB();
            var user = dbContext.Users.First();
            user.Id = dbContext.Users.First().Id;
            user.Login = dbContext.Users.First().Login;
            user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();
            dbContext.Dispose();

            return View(user);
        }
    }
}