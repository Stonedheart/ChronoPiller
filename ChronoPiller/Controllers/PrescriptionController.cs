using System;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

namespace ChronoPiller.Controllers
{
    public class PrescriptionController : Controller
    {
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(FormCollection form)
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

            var prescriptionId = dbContext.Prescriptions.FirstOrDefault(x => x.Name == prescription.Name).Id;
            dbContext.Dispose();

            return RedirectToAction("Add", "Medicine", new { id = prescriptionId});
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var dbContext = new ChronoPillerDB();
            var prescription = dbContext.Prescriptions.FirstOrDefault(y => y.Id == id);
            var prescriptedMedicines = dbContext.PrescriptedMedicines
                .Join(dbContext.MedicineBoxes,
                    prescriptedMed => prescriptedMed.MedicineBoxId,
                    medBox => medBox.Id,
                    (prescriptedMed, medBox) => new { prescriptedMed, medBox })
                .Join(dbContext.Medicines,
                    medBox => medBox.medBox.MedicineId,
                    med => med.Id,
                    (medBox, med) => new { medBox, med })
                .Select(x => new {
                    Id = x.medBox.prescriptedMed.Id,
                    Name = x.med.Name,
                    StartUsageDate = x.medBox.prescriptedMed.StartUsageDate,
                    PrescriptedBoxCount = x.medBox.prescriptedMed.PrescriptedBoxCount,
                    Dose = x.medBox.prescriptedMed.Dose,
                    Interval = x.medBox.prescriptedMed.Interval,
                    PrescriptionId = x.medBox.prescriptedMed.PrescriptionId,
                    MedicineBoxId = x.medBox.medBox.Id
                })
                .AsEnumerable()
                .Select(x => new PrescriptedMedicine
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartUsageDate = x.StartUsageDate,
                    PrescriptedBoxCount = x.PrescriptedBoxCount,
                    Dose = x.Dose,
                    Interval = x.Interval,
                    PrescriptionId = x.PrescriptionId,
                    MedicineBoxId = x.MedicineBoxId
                })
                .Where(x => x.PrescriptionId == id)
                .ToList();

            prescription.PrescriptedMedicines = prescriptedMedicines;
            dbContext.Dispose();

            return View(prescription);
        }
    }
}