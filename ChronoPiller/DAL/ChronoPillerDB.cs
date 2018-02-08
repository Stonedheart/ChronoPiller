using System.Data.Entity;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChronoPiller.DAL
{
    public class ChronoPillerDB : IdentityDbContext<User>
    {
        public ChronoPillerDB() : base("ChronoPiller.DAL.ChronoPillerDB")
        { }
        public override IDbSet<User> Users { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptedMedicine> PrescriptedMedicines { get; set; }
        public DbSet<MedicineBox> MedicineBoxes { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
    }
}