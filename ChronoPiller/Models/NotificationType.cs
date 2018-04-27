using System.ComponentModel.DataAnnotations;

namespace ChronoPiller.Models
{
    public class NotificationType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}