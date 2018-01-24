using System;
using System.Net;
using System.Net.Mail;
using System.Web.Configuration;
using System.Web.Mvc;

namespace ChronoPiller.Controllers
{
    public class NotificationController : Controller
    {
        
        [HttpGet]
        public ActionResult Check(string clientDate)
        {
            var date = new DateTime(2018, 1, 23);
            var dateString = $"{date.Day}.{date.Month}.{date.Year}";
            bool res = (clientDate.Equals(dateString));


            return Json(res, JsonRequestBehavior.AllowGet);
        }
        
        public void SendMail(string id = null)
        {
            var initClient =
                new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    Timeout = 20000,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["mailAccount"],
                        WebConfigurationManager.AppSettings["mailPassword"]),
                };
            var mail = new MailMessage("l.bielenin@gmail.com", "l.bielenin@gmail.com")
            {
                Subject = "A ZNOWU dziala przez ajax?",
                Body = (id != null) ? "Your message is: " + id : "Your message is null"
            };
            initClient.Send(mail);
        }
    }
}