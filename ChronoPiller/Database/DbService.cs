using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChronoPiller.DAL;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ChronoPiller.Database
{
    public class DbService
    {
        public ChronoUser User { get; set; }

        public DbService()
        {
            User = GetCurrentUser();
        }

        private ChronoUser GetCurrentUser()
        {
            var context = HttpContext.Current;
            var id = context.User.Identity.GetUserId();
            var user = context.GetOwinContext().GetUserManager<ChronoUserManager>().FindById(int.Parse(id));
            user.Prescriptions = GetPrescriptions(user.Id);
            return user;
        }

        public List<Prescription> GetPrescriptions(int id)
        {
            using (var db = new ChronoDbContext())
            {
                return db.Prescriptions.Where(x => x.UserId == id).ToList();
            }
        }
    }
}