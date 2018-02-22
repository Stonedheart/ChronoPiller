using System;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;
using Hangfire;

namespace ChronoPiller.Controllers
{
    public class MedicineController : Controller
    {
        public DbService Db = new DbService();

        [HttpGet]
        public ActionResult Add(int id)
        {
            return View(id);
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var prescriptionId = form["prescriptionId"];

            try
            {
                var name = form["name"];
                var startUsageDate = form["startUsageDate"];
                var interval = form["interval"];
                var dose = form["dose"];
                var prescriptedBoxCount = form["prescriptedBoxCount"];
                var activeSubstanceAmountInMg = form["activeSubstanceAmountInMg"];
                var medicineBoxCapacity = form["medicineBoxCapacity"];

                var medicine = new Medicine(name);
                Db.SaveMedToDb(medicine);

                var medicineId = Db.GetMedicineId(medicine);
                var medicineBox = new MedicineBox(medicineId, int.Parse(medicineBoxCapacity),
                    float.Parse(activeSubstanceAmountInMg));
                Db.SaveMedBoxToDb(medicineBox);

                var medicineBoxId = Db.GetMedicineBoxId(medicineId);
                var prescriptedMedicine = new PrescriptedMedicine(name, DateTime.Parse(startUsageDate).Date,
                    int.Parse(prescriptedBoxCount), int.Parse(dose), int.Parse(interval), int.Parse(prescriptionId),
                    medicineBoxId);
                Db.SavePrescriptedMedToDb(prescriptedMedicine);
                var user = new DbService().User;

                RecurringJob.AddOrUpdate(() => NotificationController.SendReminder(user.Email, Db.GetPrescriptionById(int.Parse(prescriptionId))), Cron.Daily);

                return RedirectToAction("Details", "Prescription", new {id = int.Parse(prescriptionId)});
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                var idParsed = int.TryParse(prescriptionId, out var id);
                return View(id);
            }
        }
    }
}