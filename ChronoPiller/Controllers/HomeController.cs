using System;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        private ChronoUser _currentUser;

        [Authorize]
        public ActionResult Index()
        {
            try
            {
                _currentUser = new DbService().User;
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View(_currentUser);
        }
    }
}