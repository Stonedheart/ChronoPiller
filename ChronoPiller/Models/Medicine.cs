using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChronoPiller.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public List<MedicineBox> MedicineBoxes { get; set; }

        public Medicine(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public Medicine(string name)
        {
            Name = name;
        }

        public Medicine()
        {
        }
    }
}