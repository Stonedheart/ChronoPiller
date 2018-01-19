using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["prescriptions"] == null)
            {
                Session["prescriptions"] = new List<Prescription>();
            }

            return View((List<Prescription>)Session["prescriptions"]);
        }

        [HttpGet]
        public ActionResult PrescriptionDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PrescriptionDetails(FormCollection form)
        {
            var name = form["name"];
            var prescription = new Prescription(name);

            var prescriptions = (List<Prescription>) Session["prescriptions"];
            prescriptions.Add(prescription);

            return RedirectToAction("Index");
        }
    }
}