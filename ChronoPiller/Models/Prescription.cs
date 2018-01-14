using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChronoPiller.Models
{
    public class Prescription
    {
        public string Name { get; }

        public Prescription(string name)
        {
            Name = name;
        }
    }
}