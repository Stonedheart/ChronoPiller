using System;
using System.IO;
using System.Net;
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
        public static DefaultEmailClient EmailClient = new DefaultEmailClient("l.bielenin@gmail.com", "WildCat217666");

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
            var mail = new MailMessage("l.bielenin@gmail.com", "l.bielenin@gmail.com")
            {
                Subject = "Dupadupa",
                Body = "coś innego"
            };
            EmailClient.Send(mail);
        }
    }
}