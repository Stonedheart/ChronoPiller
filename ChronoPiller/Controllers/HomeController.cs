using System;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var dbContext = new ChronoPillerDB();
            NotificationController.SendMail();

            var user = dbContext.Users.First();
            user.Id = dbContext.Users.First().Id;   
            user.Login = dbContext.Users.First().Login;
            user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();
            dbContext.Dispose();

            return View(user);
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
            var dateOfIssue = form["dateOfIssue"];
            var prescription = new Prescription(name, DateTime.Parse(dateOfIssue));

            var dbContext = new ChronoPillerDB();

            var user = dbContext.Users.First();
            user.Id = dbContext.Users.First().Id;
            user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();
            user.Prescriptions.Add(prescription);

            prescription.User = user;
            dbContext.Prescriptions.Add(prescription);
            dbContext.SaveChanges();
            dbContext.Dispose();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PrescriptionDetails(int id)
        {
            var dbContext = new ChronoPillerDB();
            var prescription = dbContext.Prescriptions.FirstOrDefault(y => y.Id == id);
            var prescriptedMedicines = dbContext.PrescriptedMedicines
                .Join(dbContext.MedicineBoxes,
                    prescriptedMed => prescriptedMed.MedicineBoxId,
                    medBox => medBox.Id,
                    (prescriptedMed, medBox) => new {prescriptedMed, medBox})
                .Join(dbContext.Medicines,
                    medBox => medBox.medBox.MedicineId,
                    med => med.Id,
                    (medBox, med) => new {medBox, med})
                .Select(x => new {
                    Id = x.medBox.prescriptedMed.Id,
                    Name = x.med.Name,
                    StartUsageDate = x.medBox.prescriptedMed.StartUsageDate,
                    PrescriptedBoxCount = x.medBox.prescriptedMed.PrescriptedBoxCount,
                    Dose = x.medBox.prescriptedMed.Dose,
                    Interval = x.medBox.prescriptedMed.Interval,
                    PrescriptionId = x.medBox.prescriptedMed.PrescriptionId,
                    MedicineBoxId = x.medBox.medBox.Id})
                .AsEnumerable()
                .Select(x => new PrescriptedMedicine {
                    Id = x.Id,
                    Name = x.Name,
                    StartUsageDate = x.StartUsageDate,
                    PrescriptedBoxCount = x.PrescriptedBoxCount,
                    Dose = x.Dose,
                    Interval = x.Interval,
                    PrescriptionId = x.PrescriptionId,
                    MedicineBoxId = x.MedicineBoxId})
                .Where(x=> x.PrescriptionId == id)
                .ToList();

            prescription.PrescriptedMedicines = prescriptedMedicines;
            dbContext.Dispose();

            return View(prescription);
        }

        [HttpGet]
        public ActionResult MedicineDetails(int id)
        {
            return View("MedicineDetails", id);
        }

        [HttpPost]
        public ActionResult MedicineDetails(FormCollection form)
        {
            var name = form["name"];
            var startUsageDate = form["startUsageDate"];
            var interval = form["interval"];
            var prescriptionId = form["prescriptionId"];
            var dose = form["dose"];
            var prescriptedBoxCount = form["prescriptedBoxCount"];
            var activeSubstanceAmountInMg = form["activeSubstanceAmountInMg"];
            var medicineBoxCapacity = form["medicineBoxCapacity"];

            var dbContext = new ChronoPillerDB();

            var medicine = new Medicine(name);
            dbContext.Medicines.Add(medicine);
            dbContext.SaveChanges();


            var medicineId = dbContext.Medicines.FirstOrDefault(x => x.Name == medicine.Name).Id;
            var medicineBox = new MedicineBox(medicineId, int.Parse(medicineBoxCapacity),
                float.Parse(activeSubstanceAmountInMg));
            dbContext.MedicineBoxes.Add(medicineBox);
            dbContext.SaveChanges();

            var medicineBoxId = dbContext.MedicineBoxes.FirstOrDefault(x => x.MedicineId == medicineId).Id;
            var prescriptedMedicine = new PrescriptedMedicine(name, DateTime.Parse(startUsageDate),
                int.Parse(prescriptedBoxCount), int.Parse(dose), int.Parse(interval), int.Parse(prescriptionId),
                medicineBoxId);
            dbContext.PrescriptedMedicines.Add(prescriptedMedicine);
            dbContext.SaveChanges();
            dbContext.Dispose();

            return RedirectToAction("PrescriptionDetails", "Home", new {id = int.Parse(prescriptionId)});
        }
    }
}