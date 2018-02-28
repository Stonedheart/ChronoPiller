using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ChronoPiller.Database
{
    public class DbService
    {
        public ChronoUser User { get; }

        public DbService()
        {
            User = GetCurrentUser();
        }

        private ChronoUser GetCurrentUser()
        {
            ChronoUser user;
            try
            {
                var context = HttpContext.Current;
                var id = context.User.Identity.GetUserId();
                user = context.GetOwinContext().GetUserManager<ChronoUserManager>().FindById(int.Parse(id));
            }
            catch (NullReferenceException)
            {
                throw new UserNotLoggedException();
            }
            user.Prescriptions = GetUserPrescriptions(user.Id);
            return user;
        }

        public Prescription GetPrescriptionById(int id)
        {
            Prescription prescription;

            using (var context = new ChronoDbContext())
            {
                prescription = context.Prescriptions.FirstOrDefault(x => x.Id == id);
            }

            prescription.PrescriptedMedicines = GetPrescriptedMedsList(id);

            return prescription;
        }

        public List<Prescription> GetUserPrescriptions(int id)
        {
            using (var db = new ChronoDbContext())
            {
                return db.Prescriptions.Where(x => x.UserId == id).ToList();
            }
        }

        public void SavePrescriptionToDb(Prescription prescription)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.Prescriptions.Add(prescription);
                dbContext.SaveChanges();
            }
        }

        public int GetPrescriptionId(Prescription prescription)
        {
            using (var dbContext = new ChronoDbContext())
            {
                return dbContext.Prescriptions.FirstOrDefault(
                        x => x.Name == prescription.Name && x.DateOfIssue == prescription.DateOfIssue)
                    .Id;
            }
        }

        public List<PrescriptedMedicine> GetPrescriptedMedsList(int id)
        {
            List<PrescriptedMedicine> prescriptedMedicines;

            using (var dbContext = new ChronoDbContext())
            {
                prescriptedMedicines = dbContext.PrescriptedMedicines
                    .Join(dbContext.MedicineBoxes,
                        prescriptedMed => prescriptedMed.MedicineBoxId,
                        medBox => medBox.Id,
                        (prescriptedMed, medBox) => new {prescriptedMed, medBox})
                    .Join(dbContext.Medicines,
                        medBox => medBox.medBox.MedicineId,
                        med => med.Id,
                        (medBox, med) => new {medBox, med})
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

                foreach (var element in prescriptedMedicines)
                {
                    element.Prescription = dbContext.Prescriptions.FirstOrDefault(x => x.Id == element.PrescriptionId);
                    element.MedicineBox = dbContext.MedicineBoxes.FirstOrDefault(x => x.Id == element.MedicineBoxId);
                }
            }

            return prescriptedMedicines;
        }

        public int GetMedicineBoxId(int medicineId)
        {
            using (var dbContext = new ChronoDbContext())
            {
                return dbContext.MedicineBoxes.FirstOrDefault(x => x.MedicineId == medicineId).Id;
            }
        }

        public int GetMedicineId(Medicine medicine)
        {
            using (var dbContext = new ChronoDbContext())
            {
                return dbContext.Medicines.FirstOrDefault(x => x.Name == medicine.Name).Id;
            }
        }

        public void SaveMedToDb(Medicine medicine)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.Medicines.Add(medicine);
                dbContext.SaveChanges();
            }
        }

        public void SaveMedBoxToDb(MedicineBox medBox)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.MedicineBoxes.Add(medBox);
                dbContext.SaveChanges();
            }
        }

        public void SavePrescriptedMedToDb(PrescriptedMedicine prescriptedMedicine)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.PrescriptedMedicines.Add(prescriptedMedicine);
                dbContext.SaveChanges();
            }
        }
    }

    internal class UserNotLoggedException : Exception
    {
    }
}