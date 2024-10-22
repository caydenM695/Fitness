using Microsoft.AspNetCore.Mvc;

namespace Fitness.Controllers
{
    public class ExerciseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddRoutine()
        {
            return View();
        }
    }
}
