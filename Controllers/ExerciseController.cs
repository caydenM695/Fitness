using Fitness.Models;
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

        [HttpPost]
        public IActionResult SaveExercise(ExerciseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var exercise = new Exercise
                {
                    dateRecorded = model.dateRecorded,
                    distanceRan = model.distanceRan,
                    time = model.time,
                    weightBenched = model.weightBenched,
                    weightDeadlift = model.weightDeadlift,
                    weightCurl = model.weightCurl
                };

                return RedirectToAction("Index");
            }

            return View(model);
        }

    }
}
