using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ChronoPiller.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime DateOfIssue { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public ChronoUser User { get; set; }

        [NotMapped]
        public List<PrescriptedMedicine> PrescriptedMedicines { get; set; }

        [NotMapped]
        public List<Dose> Doses { get; set; }

        public Prescription()
        {
            Doses = new List<Dose>();
        }

        public Prescription(string name, DateTime dateOfIssue)
        {
            Name = name;
            DateOfIssue = dateOfIssue;
            Doses = new List<Dose>();
        }

        public Prescription(int userId, string name, DateTime date)
        {
            UserId = userId;
            Name = name;
            DateOfIssue = date;
        }
    }
}