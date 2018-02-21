using System;
using System.Web.Mvc;
using ChronoPiller.Models;
using ChronoPiller.Models.Reminders;

namespace ChronoPiller.Controllers
{
    public class NotificationController : Controller
    {
        private const string MailName = "chronopiller@gmail.com";
        private const string MailPassword = "dupadupadupa";

        public static DefaultEmailClient EmailClient =
            new DefaultEmailClient(MailName, MailPassword);


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
            var factory = new EmailFactory(to);
            var mail = factory.GetEmailReminder(prescription);
            EmailClient.Send(mail);
        }

        public static void SendConfirmation(string to, Prescription prescription)
        {
            var factory = new EmailFactory(to);
            var mail = factory.GetEmailConfirmation(prescription);
            EmailClient.Send(mail);
        }
    }
}