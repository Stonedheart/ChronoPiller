using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChronoPiller.Models
{
    public class Prescription
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime DateOfIssue { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
        public List<PrescriptedMedicine> Medicines = new List<PrescriptedMedicine>();
    }
}