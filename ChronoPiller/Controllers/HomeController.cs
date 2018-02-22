using System;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        private ChronoUser _currentUser;
        private DbService _db = new DbService();

        [Authorize]
        public ActionResult Index()
        {
            try
            {
                _currentUser = _db.User;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View(_currentUser);
        }
    }
}