using System;
using System.Collections.Generic;
using System.Web.Mvc;
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
        public ActionResult AddPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPrescription(FormCollection form)
        {
            var name = form["name"];
            var prescription = new Prescription(name);

            var prescriptions = (List<Prescription>) Session["prescriptions"];
            prescriptions.Add(prescription);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PrescriptionDetails(string id)
        {
            var prescriptions = (List<Prescription>)Session["prescriptions"];

            foreach (var prescription in prescriptions)
            {
                if (Equals(prescription.Name, id))
                {
                    return View(prescription);
                }
            }
            return RedirectToAction("Index");
        }
    }
}