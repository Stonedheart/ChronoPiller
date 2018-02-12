using System.Web;
using System.Web.Mvc;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity.Owin;

namespace ChronoPiller.Controllers
{
    public class AccountController : Controller
    {
        private ChronoSignInManager _signInManager;
        private ChronoUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ChronoUserManager userManager, ChronoSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ChronoSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ChronoSignInManager>();
                
            }
            private set { _signInManager = value; }
        }

        public ChronoUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ChronoUserManager>();
                
            }
            private set { _userManager = value; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


    }
}