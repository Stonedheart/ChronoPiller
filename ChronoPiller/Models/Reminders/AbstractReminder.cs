using System;
using System.Collections;
using System.Collections.Generic;

namespace ChronoPiller.Models.Reminders
{
    public abstract class AbstractReminder
    {
        public Dictionary<string, int> PillsQuantity;
        public DateTime Time;

        public abstract void Send();

    }
}