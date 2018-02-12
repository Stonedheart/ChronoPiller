using System.Web.Mvc;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ChronoSignInManager _signInManager;
        private ChronoUserManager _userManager;

        public ManageController() { }


    }
}