using System.Net.Mail;
using System.Text;

namespace ChronoPiller.Models.Reminders
{
    public class EmailFactory
    {
        public string From;
        public string To;
        public MailMessage Email;
        private readonly StringBuilder _builder;

        public EmailFactory(string @from, string to)
        {
            From = @from;
            To = to;
            Email = new MailMessage(from, to);
            _builder = new StringBuilder();
        }

        public MailMessage GetEmailReminder(Prescription prescription)
        {
            if (_builder.Length > 0)
            {
                _builder.Clear();
            }
            prescription.PrescriptedMedicines.ForEach(x => _builder.Append(x.Name + ", "));

            Email.Subject = "Take your daily dose!";
            Email.Body = $"Hello there!\n\n" +
                         $"Your friendly neighbourhood ChronoPiller would like to remind you about " +
                         $"your daily dose of {_builder} from prescription {prescription.Name}!\n\n" +
                         $"Take it or You'll be sorry!\n\n" +
                         $"Cheers!";

            return Email;
        }

        public MailMessage GetEmailConfirmation(Prescription prescription)
        {
            Email.Subject = "A new prescription has been created!";
            Email.Body = $"Hello there!\n\n" +
                         $"Your friendly neighbourhood ChronoPiller would like to inform " +
                         $"that a new prescription {prescription.Name} has been created!\n\n" +
                         $"Take your pills or You'll be sorry!\n\n" +
                         $"Cheers!";

            return Email;
        }
    }
}