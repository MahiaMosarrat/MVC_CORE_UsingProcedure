using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace PatientManagementCore.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public bool FirstVisit { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:yyyy-MM-dd}")]
        public DateTime AppointmentDate { get; set; }
        public decimal ConsultationFee { get; set; }
        public string? ImageUrl { get; set; }
        public int SpecialistId { get; set; }
        public virtual Specialist Specialist { get; set; } = null!;
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
    public class Specialist
    {
        public int SpecialistId { get; set; }
        public string CategoryName { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string TimeSlot { get; set; } = null!;
        public int AppointmentId { get; set; }
        public virtual Appointment? Appointment { get; set; }
    }
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>op):base(op)
        {
            
        }
        public virtual DbSet<Appointment> Appointments { get; set; }
        public virtual DbSet<Specialist> Specialists { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Specialist>().HasData(
                new Specialist { SpecialistId=1,CategoryName= "General Medicine" },
                 new Specialist { SpecialistId = 2, CategoryName = "Dermatology" },
                  new Specialist { SpecialistId = 3, CategoryName = "Pediatrics" }
                );
        }
    }
}
