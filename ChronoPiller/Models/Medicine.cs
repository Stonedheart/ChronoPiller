using System;

namespace ChronoPiller.Models
{
    public class Medicine
    {
        public string Name { get; }
        public DateTime StartUseDate { get; }
        public int Interval { get; }
        
        public Medicine(string name, DateTime startUseDate, int interval)
        {
            Name = name;
            Interval = interval;
        }
    }
}