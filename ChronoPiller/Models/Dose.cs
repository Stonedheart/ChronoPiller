using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class Dose
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("MedicineBox")]
        public int MedicineBoxId { get; set; }
        public MedicineBox MedicineBox { get; set; }
        [Required]
        public int Pills { get; set; }
    }
}