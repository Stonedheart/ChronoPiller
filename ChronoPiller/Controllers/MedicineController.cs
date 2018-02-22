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
                SaveMedToDb(medicine);

                var medicineId = GetMedicineId(medicine);
                var medicineBox = new MedicineBox(medicineId, int.Parse(medicineBoxCapacity),
                    float.Parse(activeSubstanceAmountInMg));
                SaveMedBoxToDb(medicineBox);

                var medicineBoxId = GetMedicineBoxId(medicineId);
                var prescriptedMedicine = new PrescriptedMedicine(name, DateTime.Parse(startUsageDate).Date,
                    int.Parse(prescriptedBoxCount), int.Parse(dose), int.Parse(interval), int.Parse(prescriptionId),
                    medicineBoxId);
                SavePrescriptedMedToDb(prescriptedMedicine);
                var user = AccountController.GetCurrentUser();

                RecurringJob.AddOrUpdate(
                    () => NotificationController.SendReminder(user.Email,
                        GetPrescriptionById(int.Parse(prescriptionId))), Cron.Daily);

                return RedirectToAction("Details", "Prescription", new {id = int.Parse(prescriptionId)});
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                var idParsed = int.TryParse(prescriptionId, out var id);
                return View(id);
            }
        }

        private Prescription GetPrescriptionById(int id)
        {
            Prescription prescription;

            using (var context = new ChronoDbContext())
            {
                prescription = context.Prescriptions.FirstOrDefault(x => x.Id == id);


                foreach (var med in prescription.PrescriptedMedicines)
                {
                    var dose = context.Doses.FirstOrDefault(x => x.MedicineBoxId == med.MedicineBoxId);
                    prescription.Doses.Add(dose);
                }
            }

            var prescriptedMedList = PrescriptionController.GetPrescriptedMedsList(id);
            prescription.PrescriptedMedicines = prescriptedMedList;

            return prescription;
        }

        public void TakePill(int prescriptionId)
        {
            using (var context = new ChronoDbContext())
            {
                var medicines = context.PrescriptedMedicines.Where(x => x.PrescriptionId == prescriptionId);
                foreach (var med in medicines)
                {
                    med.MedicineBox.PillsInBox -= med.Dose;
                }
                context.SaveChanges();
            }
        }

        private int GetMedicineBoxId(int medicineId)
        {
            int medicineBoxId;

            using (var dbContext = new ChronoDbContext())
            {
                medicineBoxId = dbContext.MedicineBoxes.FirstOrDefault(x => x.MedicineId == medicineId).Id;
            }

            return medicineBoxId;
        }

        private int GetMedicineId(Medicine medicine)
        {
            int medicineId;

            using (var dbContext = new ChronoDbContext())
            {
                medicineId = dbContext.Medicines.FirstOrDefault(x => x.Name == medicine.Name).Id;
            }

            return medicineId;
        }

        private void SaveMedToDb(Medicine medicine)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.Medicines.Add(medicine);
                dbContext.SaveChanges();
            }
        }

        private void SaveMedBoxToDb(MedicineBox medBox)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.MedicineBoxes.Add(medBox);
                dbContext.SaveChanges();
            }
        }

        private void SavePrescriptedMedToDb(PrescriptedMedicine prescriptedMedicine)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.PrescriptedMedicines.Add(prescriptedMedicine);
                dbContext.SaveChanges();
            }
        }
    }
}