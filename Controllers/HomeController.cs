using Microsoft.AspNetCore.Mvc;

namespace PatientManagementCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
