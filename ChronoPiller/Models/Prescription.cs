using System.Collections.Generic;

namespace ChronoPiller.Models
{
    public class Prescription
    {
        public string Name { get; }
        public List<Medicine> Medicines = new List<Medicine>();

        public Prescription(string name)
        {
            Name = name;
        }
    }
}