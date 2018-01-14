using System;

namespace ChronoPiller.Models
{
    public class Prescription
    {
        public string Name { get; }
        public DateTime StartUseDate { get; }

        public Prescription(string name, DateTime startUseDate)
        {
            Name = name;
            StartUseDate = startUseDate;
        }
    }
}