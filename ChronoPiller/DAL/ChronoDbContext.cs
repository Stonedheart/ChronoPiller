using System.Data.Entity;
using ChronoPiller.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChronoPiller.DAL
{
    public class ChronoDbContext : IdentityDbContext<ChronoUser, ChronoRole,
        int, ChronoUserLogin, ChronoUserRole, ChronoUserClaim>
    {
        public ChronoDbContext() : base("ChronoPiller.DAL.ChronoPillerDB")
        { }
        public override IDbSet<ChronoUser> Users { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptedMedicine> PrescriptedMedicines { get; set; }
        public DbSet<MedicineBox> MedicineBoxes { get; set; }
        public DbSet<Medicine> Medicines { get; set; }

        public static ChronoDbContext Create()
        {
            return new ChronoDbContext();
        }
    }
}