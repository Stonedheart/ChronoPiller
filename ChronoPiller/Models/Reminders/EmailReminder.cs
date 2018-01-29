using Postal;

namespace ChronoPiller.Models.Reminders
{
    public class EmailReminder : Email
    {
        public EmailReminder(string viewName, string to, string name) : base(viewName)
        {
            To = to;
            Name = name;
        }

        public string Name { get; set; }

        public string To { get; set; }
        public string Comment { get; set; }
    }
}