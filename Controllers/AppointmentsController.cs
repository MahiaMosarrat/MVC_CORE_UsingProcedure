using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PatientManagementCore.Migrations;
using PatientManagementCore.Models;
using PatientManagementCore.Models.ViewModels;
using System.Data;

namespace PatientManagementCore.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment web;
        public AppointmentsController(AppDbContext _db, IWebHostEnvironment _web)
        {
            db = _db;
            web = _web;
        }
        public IActionResult Index()
        {
            var appointments = db.Appointments.Include(a=>a.Specialist).Include(a => a.Doctors).ToList();
            return View(appointments);
        }
        public ActionResult Delete(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                db.Appointments.Remove(appointment);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CreatePartial()
        {
            AppointmentViewModel appointment = new AppointmentViewModel();
            appointment.Specialists = db.Specialists.ToList();
            appointment.Doctors.Add(new Doctor() { DoctorId = 1 });
            return PartialView("_CreateAppointmentPartial", appointment);

        }
        
        [HttpPost]

        public async Task<ActionResult> CreateAppointment(AppointmentViewModel vobj)
        {
           
            Appointment appointment = new Appointment
            {
                PatientName = vobj.PatientName,
                Age = vobj.Age,
                Gender = vobj.Gender,
                MobileNo = vobj.MobileNo,
                FirstVisit=vobj.FirstVisit,
                AppointmentDate = vobj.AppointmentDate,
                ConsultationFee = vobj.ConsultationFee,
                SpecialistId = vobj.SpecialistId,
                Doctors=vobj.Doctors
            };

            if (vobj.ProfileFile != null)
            {
                string fileName = GetFileName(vobj.ProfileFile);
                appointment.ImageUrl = fileName;
            }
            else
            {
                appointment.ImageUrl = "noimage.png";
            }

            //db.Appointments.Add(appointment);          
            //db.SaveChanges();

                DataTable doctorTable = new DataTable();
                doctorTable.Columns.Add("DoctorName", typeof(string));
                doctorTable.Columns.Add("TimeSlot", typeof(string));
                if (appointment.Doctors != null && appointment.Doctors.Any())
                {
                    foreach (var d in appointment.Doctors)
                    {
                        doctorTable.Rows.Add(d.DoctorName, d.TimeSlot);
                    }
                }
                var parameters = new[]
                {
                   new SqlParameter("@PatientName",appointment.PatientName),
                   new SqlParameter("@Age",appointment.Age),
                   new SqlParameter("@Gender",appointment.Gender),
                   new SqlParameter("@MobileNo",appointment.MobileNo),
                   new SqlParameter("@FirstVisit",appointment.FirstVisit),
                   new SqlParameter("@AppointmentDate",appointment.AppointmentDate),
                   new SqlParameter("@ConsultationFee",appointment.ConsultationFee),
                   new SqlParameter("@SpecialistId",appointment.SpecialistId),
                   new SqlParameter("@ImageUrl",appointment.ImageUrl?? (object)DBNull.Value),

                   new SqlParameter
                   {
                       ParameterName="@Doctors",
                       SqlDbType=SqlDbType.Structured,
                       TypeName="dbo.ParamDoctorType",
                       Value=doctorTable
                   }
                };
        
            await db.Database.ExecuteSqlRawAsync("EXEC dbo.spInsertAppointment @PatientName,@Age,@Gender,@MobileNo,@FirstVisit,@AppointmentDate,@ConsultationFee,@SpecialistId,@ImageUrl,@Doctors", parameters);

            return RedirectToAction("Index");
            
        }

        private string GetFileName(IFormFile profileFile)
        {
            string fileName = null;
            if (profileFile != null)
            {
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(profileFile.FileName); ;
                var uploadFolder = Path.Combine(web.WebRootPath, "images");
                var filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profileFile.CopyToAsync(fileStream);
                }
            }
            return fileName;
        }
        [HttpGet]
        public ActionResult EditPartial(int id)
        {
            var appointment = db.Appointments.Include(a => a.Doctors).FirstOrDefault(x => x.AppointmentId == id);
            var vObj = new AppointmentViewModel
            {
                AppointmentId = appointment.AppointmentId,
                PatientName = appointment.PatientName,
                Age = appointment.Age,
                Gender = appointment.Gender,
                MobileNo = appointment.MobileNo,
                FirstVisit = appointment.FirstVisit,
                AppointmentDate = appointment.AppointmentDate,
                ConsultationFee = appointment.ConsultationFee,
                ImageUrl = appointment.ImageUrl,
                SpecialistId = appointment.SpecialistId,
                Doctors = appointment.Doctors.ToList(),
                Specialists= db.Specialists.ToList()
            };
            return PartialView("_EditAppointmentPartial", vObj);
        }

        [HttpPost]
        public async Task<ActionResult> EditAppointment(AppointmentViewModel vobj, string OldImageUrl)
        {
            if (!ModelState.IsValid)
            {
                vobj.Specialists = db.Specialists.ToList();
                return View();
            }
            Appointment appointment = db.Appointments.Find(vobj.AppointmentId);
            if (appointment != null)
            {
                appointment.PatientName = vobj.PatientName;
                appointment.Age = vobj.Age;
                appointment.Gender = vobj.Gender;
                appointment.MobileNo = vobj.MobileNo;
                appointment.FirstVisit = vobj.FirstVisit;
                appointment.AppointmentDate = vobj.AppointmentDate;
                appointment.ConsultationFee = vobj.ConsultationFee;
                appointment.SpecialistId = vobj.SpecialistId;
                if (vobj.ProfileFile != null)
                {
                    string fileName = GetFileName(vobj.ProfileFile);
                    appointment.ImageUrl = fileName;
                }
                else
                {
                    appointment.ImageUrl = OldImageUrl;
                }
                var doctors = db.Doctors.Where(d => d.AppointmentId == vobj.AppointmentId).ToList();
                DataTable doctorTable = new DataTable();
                doctorTable.Columns.Add("DoctorName", typeof(string));
                doctorTable.Columns.Add("TimeSlot", typeof(string));
                if (vobj.Doctors != null && vobj.Doctors.Any())
                {
                    foreach (var d in appointment.Doctors)
                    {
                        doctorTable.Rows.Add(d.DoctorName, d.TimeSlot);
                    }
                }
                var parameters = new[]
                {
                   new SqlParameter("@PatientName",appointment.PatientName),
                   new SqlParameter("@Age",appointment.Age),
                   new SqlParameter("@Gender",appointment.Gender),
                   new SqlParameter("@MobileNo",appointment.MobileNo),
                   new SqlParameter("@FirstVisit",appointment.FirstVisit),
                   new SqlParameter("@AppointmentDate",appointment.AppointmentDate),
                   new SqlParameter("@ConsultationFee",appointment.ConsultationFee),
                   new SqlParameter("@SpecialistId",appointment.SpecialistId),
                   new SqlParameter("@ImageUrl",appointment.ImageUrl?? (object)DBNull.Value),

                   new SqlParameter
                   {
                       ParameterName="@Doctors",
                       SqlDbType=SqlDbType.Structured,
                       TypeName="dbo.EditParamDoctorType",
                       Value=doctorTable
                   },
                    new SqlParameter("@AppointmentId",vobj.AppointmentId),
                };

                await db.Database.ExecuteSqlRawAsync("EXEC dbo.spUpdateAppointment @PatientName,@Age,@Gender,@MobileNo,@FirstVisit,@AppointmentDate,@ConsultationFee,@SpecialistId,@ImageUrl,@Doctors,@AppointmentId", parameters);

                try
                {
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {

                    vobj.Specialists = db.Specialists.ToList();
                    return View();
                }
            }
            vobj.Specialists = db.Specialists.ToList();
            return View(vobj);
        }
    }
}
