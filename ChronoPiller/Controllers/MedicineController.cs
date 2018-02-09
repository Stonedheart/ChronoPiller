using System;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Hangfire;

namespace ChronoPiller.Controllers
{
    public class MedicineController : Controller
    {
        [HttpGet]
        public ActionResult Add(int id)
        {
            return View("Add", id);
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
        {
            var name = form["name"];
            var startUsageDate = form["startUsageDate"];
            var interval = form["interval"];
            var prescriptionId = form["prescriptionId"];
            var dose = form["dose"];
            var prescriptedBoxCount = form["prescriptedBoxCount"];
            var activeSubstanceAmountInMg = form["activeSubstanceAmountInMg"];
            var medicineBoxCapacity = form["medicineBoxCapacity"];

            var medicine = new Medicine(name);
            SaveMedToDb(medicine);

            var medicineId = GetMedicineId(medicine);
            var medicineBox = new MedicineBox(medicineId, int.Parse(medicineBoxCapacity),
                float.Parse(activeSubstanceAmountInMg));
            SaveMedBoxToDb(medicineBox);

            var medicineBoxId = GetMedicineBoxId(medicineId);
            var prescriptedMedicine = new PrescriptedMedicine(name, DateTime.Parse(startUsageDate),
                int.Parse(prescriptedBoxCount), int.Parse(dose), int.Parse(interval), int.Parse(prescriptionId),
                medicineBoxId);
            SavePrescriptedMedToDb(prescriptedMedicine);

            RecurringJob.AddOrUpdate(() => NotificationController.SendReminder(GetPrescriptionById(int.Parse(prescriptionId))), Cron.Daily);
            return RedirectToAction("Details", "Prescription", new {id = int.Parse(prescriptionId)});
        }

        private Prescription GetPrescriptionById(int id)
        {
            Prescription prescription;

            using (var context = new ChronoPillerDb())
            {
                prescription = context.Prescriptions.FirstOrDefault(x => x.Id == id);
            }

            var prescriptedMedList = PrescriptionController.GetPrescriptedMedsList(id);
            prescription.PrescriptedMedicines = prescriptedMedList;

            return prescription;
        }

        private int GetMedicineBoxId(int medicineId)
        {
            int medicineBoxId;

            using (var dbContext = new ChronoPillerDb())
            {
                medicineBoxId = dbContext.MedicineBoxes.FirstOrDefault(x => x.MedicineId == medicineId).Id;
            }

            return medicineBoxId;
        }

        private int GetMedicineId(Medicine medicine)
        {
            int medicineId;

            using (var dbContext = new ChronoPillerDb())
            {
                medicineId = dbContext.Medicines.FirstOrDefault(x => x.Name == medicine.Name).Id;
            }

            return medicineId;
        }

        private void SaveMedToDb(Medicine medicine)
        {
            using (var dbContext = new ChronoPillerDb())
            {
                dbContext.Medicines.Add(medicine);
                dbContext.SaveChanges();
            }
        }

        private void SaveMedBoxToDb(MedicineBox medBox)
        {
            using (var dbContext = new ChronoPillerDb())
            {
                dbContext.MedicineBoxes.Add(medBox);
                dbContext.SaveChanges();
            }
        }

        private void SavePrescriptedMedToDb(PrescriptedMedicine prescriptedMedicine)
        {
            using (var dbContext = new ChronoPillerDb())
            {
                dbContext.PrescriptedMedicines.Add(prescriptedMedicine);
                dbContext.SaveChanges();
            }
        }
    }
}