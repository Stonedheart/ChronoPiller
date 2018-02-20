using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
            _currentUser = AccountController.GetCurrentUser();

            using (var db = new ChronoDbContext())
            {
                _currentUser.Prescriptions = db.Prescriptions.Where(x => x.UserId == _currentUser.Id).ToList();
            }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View(_currentUser);
        }
    }
}