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
        public static readonly string _mailName = MailSettingsSectionGroup["userName"];
        public static readonly string _mailPassword = MailSettingsSectionGroup["password"];

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

        public static void SendMail()
        {
            Debug.WriteLine("NAME: " + _mailName);
            Debug.WriteLine("PASS: " + _mailPassword);
            var mail = new MailMessage("l.bielenin@gmail.com", "l.bielenin@gmail.com")
            {
                Subject = "Dupadupa",
                Body = "coś innego"
            };
            EmailClient.Send(mail);
        }
    }
}