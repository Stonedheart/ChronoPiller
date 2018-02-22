using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class MedicineBox
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Medicine")]
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public int PillsInBox { get; set; }
        [Required]
        public float ActiveSubstanceAmountInMg { get; set; }

        [NotMapped]
        public List<PrescriptedMedicine> OccurancesOnPrescriptions { get; set; } 

        [NotMapped]
        public Dose Dose { get; set; }

        public MedicineBox(int medicineId, int capacity, float activeSubstanceAmountInMg)
        {
            MedicineId = medicineId;
            Capacity = capacity;
            ActiveSubstanceAmountInMg = activeSubstanceAmountInMg;
            PillsInBox = Capacity;
        }

        public MedicineBox()
        {
            
        }
    }
}