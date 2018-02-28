using System;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;
using Hangfire;

namespace ChronoPiller.Controllers
{
    public class PrescriptionController : Controller
    {

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(FormCollection form)
        {
            var Db = new DbService();
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
        [Authorize]
        public ActionResult Details(int id)
        {
            var Db = new DbService();
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

        private void TakePill(Prescription prescription)
        {
            var Db = new DbService();
            foreach (var med in prescription.PrescriptedMedicines)
            {
                if (med.MedicineBox.PillsInBox >= med.Dose)
                {
                    med.MedicineBox.PillsInBox -= med.Dose;
                }
                else
                {
                    throw new NotEnoughPillsException("Not enough pills in " + med.Name);
                }
                Db.SaveMedBoxToDb(med.MedicineBox);
            }
        }

        public void SetSchedule(Prescription prescription)
        {
            var Db = new DbService();
            var user = Db.User;
            var id = $"{user.Id}.{prescription.Id}";

            RecurringJob.AddOrUpdate(id,
                () => TakeAndRemind(prescription), Cron.Daily);
        }

        private void TakeAndRemind(Prescription prescription)
        {
            var Db = new DbService();
            var user = Db.User;
            var id = $"{user.Id}.{prescription.Id}";
            try
            {
                this.TakePill(prescription);
                NotificationController.SendReminder(user.Email, prescription);
            }
            catch (NotEnoughPillsException)
            {
                RecurringJob.RemoveIfExists(id);
            }
        }
    }

    internal class NotEnoughPillsException : Exception
    {
        public NotEnoughPillsException(string message = "There's not enough pills in the box!") : base(message)
        {
        }
    }
}