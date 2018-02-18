﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Hangfire;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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
            string name;
            string dateOfIssue;
            try
            {
                name = form["name"];
            }
            catch (NullReferenceException)
            {
                name = "Prescription: " + DateTime.Today;
            }
            try
            {
                dateOfIssue = form["dateOfIssue"];
            }
            catch (NullReferenceException)
            {
                dateOfIssue = DateTime.Today.ToString();
            }
            var prescription = new Prescription(name, DateTime.Parse(dateOfIssue));
            var user = AccountController.GetCurrentUser();

            using (var db = new ChronoDbContext())
            {
                user.Prescriptions = db.Prescriptions.Where(x => x.UserId == user.Id).ToList();
            }

            prescription.UserId = user.Id;

            user.Prescriptions.Add(prescription);

            SavePrescriptionToDb(prescription);

            BackgroundJob.Enqueue(() => NotificationController.SendConfirmation(user.Email, prescription));
            var prescriptionId = GePrescriptionId(prescription);

            return RedirectToAction("Add", "Medicine", new {id = prescriptionId});
        }

        private void SavePrescriptionToDb(Prescription prescription)
        {
            using (var dbContext = new ChronoDbContext())
            {
                dbContext.Prescriptions.Add(prescription);
                dbContext.SaveChanges();
            }
        }

        private int GePrescriptionId(Prescription prescription)
        {
            int prescriptionId;

            using (var dbContext = new ChronoDbContext())
            {
                prescriptionId = dbContext.Prescriptions.FirstOrDefault(x => x.Name == prescription.Name).Id;
            }

            return prescriptionId;
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Prescription prescription;

            using (var dbContext = new ChronoDbContext())
            {
                prescription = dbContext.Prescriptions.FirstOrDefault(y => y.Id == id);
            }

            prescription.PrescriptedMedicines = GetPrescriptedMedsList(prescription.Id);

            return View(prescription);
        }

        public static List<PrescriptedMedicine> GetPrescriptedMedsList(int id)
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
            }

            return prescriptedMedicines;
        }
    }
}