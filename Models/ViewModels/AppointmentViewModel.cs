using System.ComponentModel.DataAnnotations;

namespace PatientManagementCore.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public bool FirstVisit { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime AppointmentDate { get; set; }
        public decimal ConsultationFee { get; set; }
        public string? ImageUrl { get; set; }
        public int SpecialistId { get; set; }
        public virtual Specialist? Specialist { get; set; } = null!;
        public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
        [Display(Name ="Upload Picture")]
        public IFormFile? ProfileFile { get; set; }
        public virtual IList<Specialist>? Specialists { get; set; }
       
    }
}
