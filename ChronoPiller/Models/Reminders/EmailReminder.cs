using Postal;

namespace ChronoPiller.Models.Reminders
{
    public class EmailReminder : Email
    {
        public EmailReminder(string viewName, string to, string userName, string comment) : base(viewName)
        {
            To = to;
            UserName = userName;
            Comment = comment;
        }

        public string To { get; set; }
        public string UserName { get; set; }
        public string Comment { get; set; }
    }
}