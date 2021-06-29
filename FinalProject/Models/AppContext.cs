using System.Data.Entity;

namespace FinalProject.Models
{
    public class MyAppContext : DbContext
    {
        public MyAppContext() : base("name=DefaultConnection")
        {

        }

        public DbSet<TokenProperties> Tokens { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Certifcate> Certifcates { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicType> ClinicTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineType> MedicineTypes { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<PharmacyMedicines> PharmacyMedicines { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserType> Usertypes { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
    }
}