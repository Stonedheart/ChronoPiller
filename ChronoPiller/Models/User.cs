using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin.Security.Providers.Orcid.Message;

namespace ChronoPiller.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [NotMapped]
        public List<Prescription> Prescriptions { get; set; }

        public User() { }
    

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }

    public class Role : IdentityRole
    {
        public Role() : base()
        {
        }

        public Role(string name) : base(name)
        {
        }
    }
}