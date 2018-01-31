using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [NotMapped]
        public List<Prescription> Prescriptions { get; set; }

        public User()
        {
        }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}