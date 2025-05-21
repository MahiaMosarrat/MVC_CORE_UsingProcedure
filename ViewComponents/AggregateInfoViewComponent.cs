using Microsoft.AspNetCore.Mvc;
using PatientManagementCore.Models.ViewModels;
using PatientManagementCore.Models;

namespace PatientManagementCore.ViewComponents
{
    public class AggregateInfoViewComponent:ViewComponent
    {
        private readonly AppDbContext db;
        public AggregateInfoViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var data = db.Appointments.Join(db.Specialists,
            appointment => appointment.SpecialistId,
            category => category.SpecialistId,
            (appointment, category) => new { Appointment = appointment, Category = category })
        .ToList();
            if (data.Count > 0)
            {
                var min = data.Min(s => s.Appointment.ConsultationFee);
                var max = data.Max(s => s.Appointment.ConsultationFee);
                var sum = data.Sum(s => s.Appointment.ConsultationFee);
                var avg = data.Average(s => s.Appointment.ConsultationFee);
                var count = data.Count();

                var groupByResult = data
                    .GroupBy(s => new { s.Appointment.SpecialistId, s.Category.CategoryName }).Select(c => new GroupByViewModel
                    {
                        SpecialistId = c.Key.SpecialistId,
                        CategoryName = c.Key.CategoryName,
                        MaxValue = c.Max(s => s.Appointment.ConsultationFee),
                        MinValue = c.Min(s => s.Appointment.ConsultationFee),
                        SumValue = c.Sum(s => s.Appointment.ConsultationFee),
                        AvgValue = c.Average(s => s.Appointment.ConsultationFee),
                        Count = c.Count()
                    }).ToList();

                var aggregateViewModel = new AggregateViewModel
                {
                    MinValue = min,
                    MaxValue = max,
                    SumValue = sum,
                    AvgValue = avg,
                    GroupByResult = groupByResult
                };
                return View(aggregateViewModel);
            }
            return View();

        }
    }
}
