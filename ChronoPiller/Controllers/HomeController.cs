using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace ChronoPiller.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SendMail(string id = null)
        {
            var initClient =
                new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 10000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("l.bielenin@gmail.com", "WildCat217666"),
                };
            var mail = new MailMessage("l.bielenin@gmail.com", "l.bielenin@gmail.com")
            {
                Subject = "Hej, Tatiana!",
                Body = (id != null) ? "The Message is " + id : "The null message"
            };
            initClient.Send(mail);
            return Redirect("http://joemonster.org/");
        }
    }
}