using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class PrescriptedMedicine
    {
        public PrescriptedMedicine(string name, DateTime start, int interval)
        {
            Name = name;
            StartUsageDate = start;
            Interval = interval;
        }

        [Key]
        public int Id { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [Required]
        public DateTime StartUsageDate { get; set; }
        [Required]
        public int PrescriptedBoxCount { get; set; }
        [Required]
        public int Dose { get; set; }
        [Required]
        public int Interval { get; set; }
        [ForeignKey("Prescription")]
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }
        [ForeignKey("MedicineBox")]
        public int MedicineBoxId { get; set; }
        public MedicineBox MedicineBox { get; set; }

        public PrescriptedMedicine(string name, DateTime startUsageDate, int prescriptedBoxCount, int dose, int interval, int prescriptionId, int medicineBoxId)
        {
            Name = name;
            StartUsageDate = startUsageDate;
            PrescriptedBoxCount = prescriptedBoxCount;
            Dose = dose;
            Interval = interval;
            PrescriptionId = prescriptionId;
            MedicineBoxId = medicineBoxId;
        }

        public PrescriptedMedicine()
        {
            
        }
    }
}