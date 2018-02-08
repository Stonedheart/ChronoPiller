using System;
using System.Linq;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

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

            return RedirectToAction("Details", "Prescription", new { id = int.Parse(prescriptionId) });
        }
    }
}