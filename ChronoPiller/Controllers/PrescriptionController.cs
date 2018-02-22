using System;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;
using Hangfire;

namespace ChronoPiller.Controllers
{
    public class PrescriptionController : Controller
    {
        public DbService Db = new DbService();

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            try
            {
                var name = form["name"] ?? "Prescription: " + DateTime.Today;
                var dateOfIssue = form["dateOfIssue"] ?? DateTime.Today.ToString();
                var prescription = new Prescription(name, DateTime.Parse(dateOfIssue).Date);
                var user = Db.User;

                prescription.UserId = user.Id;

                user.Prescriptions.Add(prescription);

                Db.SavePrescriptionToDb(prescription);

            BackgroundJob.Enqueue(() => NotificationController.SendConfirmation(user.Email, prescription));
            var prescriptionId = Db.GetPrescriptionId(prescription);

                return RedirectToAction("Add", "Medicine", new {id = prescriptionId});
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Prescription prescription = null;

            try
            {
                prescription = Db.GetPrescriptionById(id);
            }
            catch (Exception e)
            {
               ViewBag.ErrorMessage = e.Message;
            }
            return View(prescription);
        }
    }
}