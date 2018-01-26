using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dbContext = new ChronoPillerDB();
            var user = new User("TestUser", "1234");
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return View(user);
        }
    }
}