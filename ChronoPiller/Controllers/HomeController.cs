﻿using System;
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

        [HttpGet]
        public ActionResult MedicineDetails(string id)
        {
            return View("MedicineDetails", (object)id);
        }

        [HttpPost]
        public ActionResult MedicineDetails(FormCollection form)
        {
            var name = form["name"];
            var startUseDate = form["startUseDate"];
            var interval = form["interval"];
            var prescriptionId = form["prescriptionId"];

            var medicine = new Medicine(name, DateTime.Parse(startUseDate), int.Parse(interval));
            var prescriptions = (List<Prescription>) Session["prescriptions"];

            foreach (var prescription in prescriptions)
            {
                if (prescription.Name == prescriptionId)
                {
                    prescription.Medicines.Add(medicine);
                }
            }

            return RedirectToAction("PrescriptionDetails", "Home", new { id = prescriptionId });
        }
    }
}