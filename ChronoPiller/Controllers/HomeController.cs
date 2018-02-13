using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Owin.Security.Providers.Orcid.Message;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {

        internal ChronoUser _currentUser;

        [Authorize]
        public ActionResult Index()
        {

            _currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ChronoUserManager>()
                .FindById(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.GetUserId()));
            using (var db = new ChronoDbContext()) { 
            _currentUser.Prescriptions = db.Prescriptions.Where(x => x.)

            return View(user);
        }
    }
}