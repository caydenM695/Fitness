using Fitness.Data;
using Fitness.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fitness.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddRoutine()
        {
            return View();
        }

        public IActionResult ViewExercises()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userExercises = _context.Exercises
                .Where(e => e.userId == userId)
                .ToList();

            ViewBag.UserExercises = userExercises;

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
                    weightCurl = model.weightCurl,
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                };

                _context.Exercises.Add(exercise);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }


            return View("AddRoutine", model);
        }
    }
}
