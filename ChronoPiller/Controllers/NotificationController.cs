using System;
using System.Web.Mvc;
using ChronoPiller.Models;
using ChronoPiller.Models.Reminders;

namespace ChronoPiller.Controllers
{
    public class NotificationController : Controller
    {
        private static EmailFactory _emailFactory;
        private static readonly ChronoEmailService Service = new ChronoEmailService();


        [HttpGet]
        public ActionResult Check(string clientDate)
        {
            var date = new DateTime(2018, 1, 23);
            var dateString = $"{date.Day}.{date.Month}.{date.Year}";
            bool res = (clientDate.Equals(dateString));

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public static void SendReminder(string to, Prescription prescription)
        {
            _emailFactory = new EmailFactory(to);
            var mail = _emailFactory.GetEmailReminder(prescription);
            Service.SendAsync(mail.ToIdentityMessage());
        }

        public static void SendConfirmation(string to, Prescription prescription)
        {
            _emailFactory = new EmailFactory(to);
            var mail = _emailFactory.GetEmailConfirmation(prescription);
            Service.SendAsync(mail.ToIdentityMessage());
        }

        public static void SendWarning(string to, Prescription prescription)
        {
            _emailFactory = new EmailFactory(to);
            var mail = _emailFactory.GetEmailWarning(prescription);
            Service.SendAsync(mail.ToIdentityMessage());
        }
    }
}