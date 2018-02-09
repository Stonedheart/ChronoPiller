using System;
using System.Collections.Generic;
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

            var user = HomeController.GetDefaultUser();
            user.Prescriptions.Add(prescription);

            prescription.User = user;
            SavePrescriptionToDb(prescription);
            var prescriptionId = GePrescriptionId(prescription);

            return RedirectToAction("Add", "Medicine", new { id = prescriptionId });
        }

        private void SavePrescriptionToDb(Prescription prescription)
        {
            using (ChronoPillerDB dbContext = new ChronoPillerDB())
            {
                dbContext.Prescriptions.Add(prescription);
                dbContext.SaveChanges();
            }
        }

        private int GePrescriptionId(Prescription prescription)
        {
            int prescriptionId;

            using (ChronoPillerDB dbContext = new ChronoPillerDB())
            {
                prescriptionId = dbContext.Prescriptions.FirstOrDefault(x => x.Name == prescription.Name).Id;
            }

            return prescriptionId;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Prescription prescription;

            using (ChronoPillerDB dbContext = new ChronoPillerDB())
            {
                prescription = dbContext.Prescriptions.FirstOrDefault(y => y.Id == id);
            }

            prescription.PrescriptedMedicines = GetPrescriptedMedsList(prescription.Id);

            return View(prescription);
        }

        private List<PrescriptedMedicine> GetPrescriptedMedsList(int id)
        {
            List<PrescriptedMedicine> prescriptedMedicines;

            using (ChronoPillerDB dbContext = new ChronoPillerDB())
            {
                prescriptedMedicines = dbContext.PrescriptedMedicines
                    .Join(dbContext.MedicineBoxes,
                        prescriptedMed => prescriptedMed.MedicineBoxId,
                        medBox => medBox.Id,
                        (prescriptedMed, medBox) => new { prescriptedMed, medBox })
                    .Join(dbContext.Medicines,
                        medBox => medBox.medBox.MedicineId,
                        med => med.Id,
                        (medBox, med) => new { medBox, med })
                    .Select(x => new
                    {
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
            }

            return prescriptedMedicines;
        }
    }
}