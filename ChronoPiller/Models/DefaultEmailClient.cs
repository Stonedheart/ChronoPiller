using System.Net;
using System.Net.Mail;

namespace ChronoPiller.Models
{
    public class DefaultEmailClient : SmtpClient
    {
        private const string Host = "smtp.gmail.com";
        private const int Port = 587;
        private const bool EnableSsl = true;
        public int Timeout = 20000;
        private const SmtpDeliveryMethod DeliveryMethod = SmtpDeliveryMethod.Network;
        private const bool UseDefaultCredentials = false;
        public NetworkCredential Credentials;

        public DefaultEmailClient(string defaultName, string defaultPassword)
        {
            Credentials = new NetworkCredential(defaultName, defaultPassword);
        }
    }
}