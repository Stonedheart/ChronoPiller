using System.Data.Entity;
using ChronoPiller.Models;

namespace ChronoPiller.DAL
{
    public class ChronoPillerDB : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptedMedicine> PrescriptedMedicines { get; set; }
        public DbSet<MedicineBox> MedicineBoxes { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }
}