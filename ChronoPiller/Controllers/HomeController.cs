using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChronoPiller.Models;
using ChronoPiller.Models.Reminders;
using Hangfire;

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

            return View((List<Prescription>) Session["prescriptions"]);
        }

        [HttpGet]
        public ActionResult AddPrescription()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPrescription(FormCollection form)
        {
            if (Session["prescriptions"] == null)
            {
                Session["prescriptions"] = new List<Prescription>();
            }

            var name = form["name"];
            var userEmail = form["email"];
            var prescription = new Prescription(name);


            var prescriptions = (List<Prescription>) Session["prescriptions"];
            prescriptions.Add(prescription);

            var reminderEmail = new EmailReminder
            {
                To = userEmail,
                Name = prescription.Name,
                ViewName = "EmailReminder"
            };
            var confirmationEmail = new EmailReminder
            {
                To = userEmail,
                Name = prescription.Name,
                ViewName = "EmailConfirmation"
            }; 

            var jobId = $"{prescription.Name}";
            var cronDailyAt12 = @"0 0 12 1/1 * ? *";

            RecurringJob.AddOrUpdate(() => System.Diagnostics.Debug.WriteLine("co jakis czas cokolwiek"), Cron.Minutely);
                

            RecurringJob.AddOrUpdate(() => NotificationController.SendMail("dupadupa"), Cron.Minutely);
            BackgroundJob.Enqueue(() => NotificationController.SendEmail(confirmationEmail));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PrescriptionDetails(string id)
        {
            var prescriptions = (List<Prescription>) Session["prescriptions"];
            try
            {
                foreach (var prescription in prescriptions)
                {
                    if (Equals(prescription.Name, id))
                    {
                        return View(prescription);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MedicineDetails(string id)
        {
            return View("MedicineDetails", (object) id);
        }

        [HttpPost]
        public ActionResult MedicineDetails(FormCollection form)
        {
            var name = form["name"];
            var startUseDate = form["startUseDate"];
            var interval = form["interval"];
            var prescriptionId = form["prescriptionId"];

            var medicine = new PrescriptedMedicine(name, DateTime.Parse(startUseDate), int.Parse(interval));
            var prescriptions = (List<Prescription>) Session["prescriptions"];

            foreach (var prescription in prescriptions)
            {
                if (prescription.Name == prescriptionId)
                {
                    prescription.Medicines.Add(medicine);
                }
            }

            return RedirectToAction("PrescriptionDetails", "Home", new {id = prescriptionId});
        }
    }
}