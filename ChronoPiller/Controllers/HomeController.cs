using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

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
                    Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["mailAccount"],
                        WebConfigurationManager.AppSettings["mailPassword"]),
                };
            var mail = new MailMessage("l.bielenin@gmail.com", "l.bielenin@gmail.com")
            {
                Subject = "Tak to się robi!",
                Body = (id != null) ? "The Message is " + id : "The null message"
            };
            initClient.Send(mail);
            return Redirect("http://gmail.com/");
        }

        public ActionResult Movies()
        {
            var movies = new List<object>();

            movies.Add(new {Title = "Ghostbusters", Genre = "Comedy", Year = 1984});
            movies.Add(new {Title = "Gone with Wind", Genre = "Drama", Year = 1939});
            movies.Add(new {Title = "Star Wars", Genre = "Science Fiction", Year = 1977});

            return Json(movies, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult Remind()
        {
            var date = new DateTime(2017, 12, 1);
            var dict = new Dictionary<string, string> {{"2", date.ToString()}};
            var json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}