using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using ChronoPiller.DAL;
using ChronoPiller.Models;

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

        public static void SendReminder()
        {
            var mail = new MailMessage("chrono@piller.com", "l.bielenin@gmail.com")
            {
                Subject = "Tak yo pill",
                Body = "It is a high time to take your prescripted medicine!"
            };
            EmailClient.Send(mail);
        }
    }
}