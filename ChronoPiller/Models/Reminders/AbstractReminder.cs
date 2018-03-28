using System;

namespace ChronoPiller.Models.Reminders
{
    public abstract class AbstractReminder
    {
        public int UserId { get; set; }
        public Prescription Prescription { get; set; }
        public int TypeId { get; set; } // DOROBIC TAJPY!
        public string JobId { get; set; }
        public string Cron { get; set; }
        public DateTime NextExecutionDate { get; set; }
    }
}