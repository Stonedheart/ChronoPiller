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
using Newtonsoft.Json.Linq;

namespace ChronoPiller.Controllers
{
    public class Person
    {
        public string Name;
        public int Age;
        public string Description;

        public Person(string name, int age, string description)
        {
            Name = name;
            Age = age;
            Description = description;
        }
    }

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

//        public ActionResult OrderToSentMail()
//        {
//            return Redirect("/Home/SendMail");
//        }
//
//
//        public ActionResult SampleJson()
//        {
//            var person1 = new Person("Lucas", 21, "Happy fellow");
//            var person2 = new Person("Andrzej", 42, "Non happy fellow");
//            var json2 = JsonConvert.SerializeObject(person2);
//            var list = new List<Person>() {person1, person2};
//
//
//            return Json(list, JsonRequestBehavior.AllowGet);
//        }
//
//
//        public ActionResult Movies()
//        {
//            var movies = new List<object>();
//
//            movies.Add(new {Title = "Ghostbusters", Genre = "Comedy", Year = 1984});
//            movies.Add(new {Title = "Gone with Wind", Genre = "Drama", Year = 1939});
//            movies.Add(new {Title = "Star Wars", Genre = "Science Fiction", Year = 1977});
//
//            return Json(movies, JsonRequestBehavior.AllowGet);
//        }
    }
}