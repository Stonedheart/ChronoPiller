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
        public User User { get; set; }
        [Required]
        public DateTime DateOfIssue { get; set; }
        [NotMapped]
        public List<PrescriptedMedicine> Medicines { get; set; }

        public Prescription()
        {
        }

        public Prescription(string name, DateTime dateOfIssue)
        {
            Name = name;
            DateOfIssue = dateOfIssue;
        }

    }
}