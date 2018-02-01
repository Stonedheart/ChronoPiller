using System.Net.Mail;
using Newtonsoft.Json;

namespace ChronoPiller.Models.Reminders
{
    public class EmailReminder : MailMessage
    {
        public string ReceiverName { get; set; }

        public EmailReminder(string @from, string to, 
            string subject, string body, string name) : base(@from, to, subject, body)
        {
            ReceiverName = name;
        }
    }
}