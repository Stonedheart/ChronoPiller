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

        private static EmailFactory _emailFactory;


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
            EmailClient.Send(mail);
        }

        public static void SendConfirmation(string to, Prescription prescription)
        {
            _emailFactory = new EmailFactory(to);
            var mail = _emailFactory.GetEmailConfirmation(prescription);
            EmailClient.Send(mail);
        }

        public static void SendWarning(string to, Prescription prescription)
        {
            _emailFactory = new EmailFactory(to);
            var mail = _emailFactory.GetEmailWarning(prescription);
            EmailClient.Send(mail);
        }
    }
}