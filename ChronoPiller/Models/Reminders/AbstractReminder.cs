using System;

namespace ChronoPiller.Models.Reminders
{
    public abstract class AbstractReminder
    {
        public int UserId { get; set; }
        public Prescription Prescription { get; set; }
        public DateTime Time { get; set; }
    }
}