using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ChronoPiller.Database;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using CsQuery.ExtensionMethods.Internal;
using Hangfire;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;

namespace ChronoPiller.Controllers
{
    public class PrescriptionController : Controller
    {
        private DbService _service;

        private DbService DbService
        {
            get => _service ?? new DbService();
            set => _service = value;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

//        [HttpPost]
//        [Authorize]
//        public ActionResult Add(FormCollection form)
//        {
//            try
//            {
//                var name = form["name"] ?? "Prescription: " + DateTime.Today;
//                var dateOfIssue = form["dateOfIssue"] ?? DateTime.Today.ToString();
//                var prescription = new Prescription(name, DateTime.Parse(dateOfIssue).Date);
//                var user = DbService.User;
//
//                prescription.UserId = user.Id;
//
//                user.Prescriptions.Add(prescription);
//
//                DbService.SavePrescriptionToDb(prescription);
//
//                BackgroundJob.Enqueue(() => NotificationController.SendConfirmation(user.Email, prescription));
//                var prescriptionId = DbService.GetPrescriptionId(prescription);
//
//                return RedirectToAction("Add", "Medicine", new {id = prescriptionId});
//            }
//            catch (Exception e)
//            {
//                ViewBag.ErrorMessage = e.Message;
//                return View();
//            }
//        }

        [HttpPost]
        public async Task<JsonResult> AddPrescriptionAsync(string json)
        {
            var dict = new Dictionary<string, string> {{"message", "success"}};
            if (json == null)
            {
                throw new ArgumentNullException();
            }

            try
            {
                var result = AddAsync(json);
                await result;

                return Json(dict);
            }
            catch (Exception e)
            {
                dict["message"] = e.Message;
                return Json(dict);
            }
        }

        private async Task AddAsync(string json)
        {
            var jsonObject = JObject.Parse(json);
            var prescriptionData = jsonObject["prescription"];
            var medicinesData = jsonObject["medicines"].ToList();

            var context = new ChronoDbContext();

            var prescriptionId = SavePrescriptionFromJsonAsync(prescriptionData["name"].ToString(),
                prescriptionData["dateOfIssue"].ToString(), context);

            var medicineIds = new List<int>();
            medicinesData.ForEach(async x =>
                medicineIds.Add(await SaveMedicineFromJsonAsync(x["name"].ToString(), context)));



            for (var i = 0; i < medicineIds.Count; i++)
            {
                var medicineBoxId = SaveMedicineBoxFromJsonAsync(medicineIds[i],
                    medicinesData[i]["medicineBoxCapacity"].ToString(),
                    medicinesData[i]["activeSubstanceAmountInMg"].ToString(), context);

                await SavePrescriptedMedicineFromJsonAsync(medicinesData[i]["name"].ToString(),
                    medicinesData[i]["startUsageDate"].ToString(),
                    medicinesData[i]["prescriptedBoxCount"].ToString(),
                    medicinesData[i]["dose"].ToString(),
                    await prescriptionId,
                    await medicineBoxId, context);
            }

            context.Dispose();
        }

        private async Task<int> SavePrescriptionFromJsonAsync(string name, string dateOfIssue, ChronoDbContext context)
        {
            if (name.IsNullOrEmpty())
            {
                throw new NullNameException("Prescription has to have a name!");
            }

            DateTime date;
            try
            {
                date = DateTime.Parse(dateOfIssue);
            }
            catch (FormatException)
            {
                throw new InvalidDateException("Prescription date is invalid!");
            }
            var userId = int.Parse(User.Identity.GetUserId());
            var prescription = new Prescription(userId, name, date);
            try
            {
                context.Prescriptions.Add(prescription);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                using (var writer = new StreamWriter("C:\\jsws\\entityLog.txt"))
                {
                    writer.Write(e);
                }
                throw new EntityException(e.Message);
            }

            return prescription.Id;
        }


        private async Task<int> SaveMedicineFromJsonAsync(string name, ChronoDbContext context)
        {
            if (name.IsNullOrEmpty())
            {
                throw new NullNameException("Medicine name cannot be null!");
            }
            var medicine =
                new Medicine(name);
            context.Medicines.Add(medicine);
            context.SaveChanges();

            var id = medicine.Id;

            return id;
        }

        private async Task<int> SaveMedicineBoxFromJsonAsync(int medicineId, string capacity,
            string activeSubstanceAmountInMg, ChronoDbContext context)
        {
            int boxCapacity;
            try
            {
                boxCapacity = int.Parse(capacity);
            }
            catch (ArgumentNullException ex)
            {
                ex = new ArgumentNullException("Box's capacity can't be null!");

                throw ex;
            }
            float boxActiveSubstance;
            try
            {
                boxActiveSubstance = float.Parse(activeSubstanceAmountInMg);
            }
            catch (ArgumentNullException ex)
            {
                ex = new ArgumentNullException("Active substance can't be null!");

                throw ex;
            }
            var medicineBox = new MedicineBox(medicineId, boxCapacity, boxActiveSubstance);
            context.MedicineBoxes.Add(medicineBox);

            var id = medicineBox.Id;

            return id;
        }

        private async Task SavePrescriptedMedicineFromJsonAsync(string name, string startUsageDate,
            string prescriptedBoxCount, string dose, int prescriptionId, int medicineBoxId, ChronoDbContext context)
        {
            if (name.IsNullOrEmpty())
            {
                throw new NullNameException("Name cannot be empty!");
            }
            DateTime medicineUsageDate;
            try
            {
                medicineUsageDate = DateTime.Parse(startUsageDate).Date;
            }
            catch (ArgumentNullException ex)
            {
                ex = new ArgumentNullException("Start usage date of medicine cannot be null!");
                throw ex;
            }
            catch (FormatException format)
            {
                format = new FormatException("Start usage date of med has ivalid format!");
                throw format;
            }
            int boxCount;
            try
            {
                boxCount = int.Parse(prescriptedBoxCount);
            }
            catch (ArgumentNullException ex)
            {
                ex = new ArgumentNullException("Prescripted box count can't be empty!");
                throw ex;
            }
            catch (FormatException format)
            {
                format = new FormatException("Invalid box count format!");
                throw format;
            }

            int prescriptedDose;
            try
            {
                prescriptedDose = int.Parse(dose);
            }
            catch (ArgumentNullException ex)
            {
                ex = new ArgumentNullException("Dose can't be empty!");
                throw ex;
            }
            catch (FormatException format)
            {
                format = new FormatException("Invalid dose format!");
                throw format;
            }

            var prescriptedMedicine = new PrescriptedMedicine(name,
                medicineUsageDate,
                boxCount,
                prescriptedDose,
                prescriptionId,
                medicineBoxId);
            context.PrescriptedMedicines.Add(prescriptedMedicine);
            context.SaveChanges();
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
                SetSchedule(prescription);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View(prescription);
        }

        private void TakePill(Prescription prescription)
        {
            var Db = new DbService(prescription.UserId);
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
                () => TakeAndRemind(user.Id, prescription), "12 15 * * *");
        }

        public void TakeAndRemind(int userId, Prescription prescription)
        {
            var user = DbService.GetUserById(userId);
            var id = $"{user.Id}.{prescription.Id}";
            try
            {
                this.TakePill(prescription);
                NotificationController.SendReminder(user.Email, prescription);
            }
            catch (NotEnoughPillsException)
            {
                NotificationController.SendWarning(user.Email, prescription);
                RecurringJob.RemoveIfExists(id);
            }
        }
    }

    internal class InvalidDateException : Exception
    {
        public InvalidDateException(string message)
        {
        }
    }

    internal class NotEnoughPillsException : Exception
    {
        public NotEnoughPillsException(string message = "There's not enough pills in the box!") : base(message)
        {
        }
    }

    internal class NullNameException : Exception
    {
        public NullNameException()
        {
        }

        public NullNameException(string message) : base(message)
        {
        }
    }
}
