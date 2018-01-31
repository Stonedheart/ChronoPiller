using Newtonsoft.Json;
using Postal;

namespace ChronoPiller.Models.Reminders
{
    public class EmailReminder : Email
    {
        public string Name { get; set; }

        public string To { get; set; }
    }
}