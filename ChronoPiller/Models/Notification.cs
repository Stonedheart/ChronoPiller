using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public ChronoUser User { get; set; }
        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        [ForeignKey("NotificationType")]
        public int NotificationTypeId { get; set; }
        public string HangFireId { get; set; }
        public string Cron { get; set; }
        public DateTime NextExecution { get; set; }
    }
}