using System.Web;
using System.Web.Mvc;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity.Owin;

namespace ChronoPiller.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ChronoSignInManager _signInManager;
        private ChronoUserManager _userManager;

        public ManageController() { }

        public ManageController(ChronoSignInManager signInManager, ChronoUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public ChronoSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ChronoSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ChronoUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ChronoUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}