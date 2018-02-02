using System.Net.Mail;

namespace ChronoPiller.Models.Reminders
{
    public class EmailReminder : MailMessage
    {
        public Prescription Prescription { get; set; }

        public EmailReminder(string @from, string to, 
            string subject, string body, Prescription prescription) : base(@from, to, subject, body)
        {
            Prescription = prescription;
        }
    }
}