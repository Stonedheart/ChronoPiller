using System;
using System.Collections;
using System.Collections.Generic;

namespace ChronoPiller.Models.Reminders
{
    public abstract class AbstractReminder
    {
        public int UserId { get; set; }
        public Dictionary<string, int> PillsQuantity { get; set; }
        public DateTime Time { get; set; }

        public abstract void Send();
    }
}