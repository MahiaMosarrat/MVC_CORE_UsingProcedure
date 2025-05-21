using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientManagementCore.Models;

namespace PatientManagementCore.ViewComponents
{
    public class HeadCountViewComponent: ViewComponent
    {
        private readonly AppDbContext db;
        public HeadCountViewComponent(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<IViewComponentResult> InvokeAsync(int specialistId)
        {
            var categoryCounts = await db.Appointments.Include(s => s.Specialist)
                .GroupBy(s => new { s.Specialist.SpecialistId, s.Specialist.CategoryName })
                .Select(g => new CategoryHeadCount
                {
                    SpecialistId = g.Key.SpecialistId,
                    CategoryName = g.Key.CategoryName,
                    Count = g.Count()
                })
                .ToListAsync();
            return View(categoryCounts);

        }
    }
}
