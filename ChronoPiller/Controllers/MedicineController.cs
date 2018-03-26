using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.Models;
using Newtonsoft.Json.Linq;

namespace ChronoPiller.Controllers
{
    public class MedicineController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Add(int id)
        {
            return View(id);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(FormCollection form)
        {
            var prescriptionId = form["prescriptionId"];
            var Db = new DbService();

            try
            {
                var name = form["name"];
                var startUsageDate = form["startUsageDate"];
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
                    int.Parse(prescriptedBoxCount), int.Parse(dose), int.Parse(prescriptionId),
                    medicineBoxId);
                Db.SavePrescriptedMedToDb(prescriptedMedicine);

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
