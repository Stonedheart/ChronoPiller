using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChronoPiller.Models
{
    public class Prescription
    {
        public string Name { get; }
        public DateTime StartUseDate { get; }
        public List<Medicine> Medicines = new List<Medicine>();

        public Prescription(string name, DateTime startUseDate)
        {
            Name = name;
            StartUseDate = startUseDate;
        }
    }
}