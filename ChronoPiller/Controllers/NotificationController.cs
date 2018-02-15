using System;
using System.Web.Mvc;
using ChronoPiller.Models;
using ChronoPiller.Models.Reminders;

namespace ChronoPiller.Controllers
{
    public class NotificationController : Controller
    {
        public static readonly string _mailName = "chronopiller@gmail.com";
        public static readonly string _mailPassword = "dupadupadupa";

        public static DefaultEmailClient EmailClient =
            new DefaultEmailClient(_mailName, _mailPassword);


        [HttpGet]
        public ActionResult Check(string clientDate)
        {
            var date = new DateTime(2018, 1, 23);
            var dateString = $"{date.Day}.{date.Month}.{date.Year}";
            bool res = (clientDate.Equals(dateString));

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public static void SendReminder(Prescription prescription)
        {
            var mail = EmailFactory.GetEmailReminder(prescription);
            EmailClient.Send(mail);
        }

        public static void SendConfirmation(Prescription prescription)
        {
            var mail = EmailFactory.GetEmailConfirmation(prescription);
            EmailClient.Send(mail);
        }
    }
}