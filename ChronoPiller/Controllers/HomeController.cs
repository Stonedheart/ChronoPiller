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
            var user = dbContext.Users.First();
            user.Id = dbContext.Users.First().Id;
            user.Login = dbContext.Users.First().Login;
            user.Prescriptions = dbContext.Prescriptions.Select(x => x).ToList();

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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult PrescriptionDetails(string id)
        {
            var dbContext = new ChronoPillerDB();
            var prescription = dbContext.Prescriptions.FirstOrDefault(y => y.Name ==id);

            return View(prescription);
        }
    }
}